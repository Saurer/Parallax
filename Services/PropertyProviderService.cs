using System.Linq;
using System.Threading.Tasks;
using AuroraCore;
using AuroraCore.Storage;
using Parallax.Models;

namespace Parallax.Services {
    public class PropertyProviderService {
        private readonly DialogService dialog;
        private readonly IStorageAPI storage;
        private readonly TransactionsService tx;

        public PropertyProviderService(DialogService dialog, IStorageAPI storage, TransactionsService tx) {
            this.dialog = dialog;
            this.storage = storage;
            this.tx = tx;
        }

        public async Task<AttachedAttrData> GetAttachedAttribute(int providerID, int attributeID) {
            var attr = await storage.GetPropertyProviderAttribute(providerID, attributeID);
            return await GetAttachedAttribute(attr);

        }

        public async Task<AttachedAttrData> GetAttachedAttribute(IAttachedProperty<IAttr> attr) {
            var plainAttr = await attr.GetProperty();
            var required = await attr.IsRequired();
            var cardinality = await attr.GetCardinality();
            var permission = await attr.GetPermission();
            var provider = await GetProvider(attr.AttachmentID);
            var attrData = await AttrData.Instantiate(plainAttr);
            var defaultValue = await attr.GetDefaultValue();
            IBoxedValue boxedDefaultValue = null;
            if (null != defaultValue) {
                var shownValue = attrData.Values.Where(a => a.EventID.ToString() == defaultValue).SingleOrDefault();
                boxedDefaultValue = new BoxedValueData(defaultValue, shownValue?.Value ?? defaultValue);
            }
            return new AttachedAttrData(attrData, required, cardinality, permission, boxedDefaultValue, provider, attr.Conditions, attr.AttachmentID);
        }

        public async Task<AttachedRelationData> GetAttachedRelation(IAttachedProperty<IRelation> relation) {
            var plainRelation = await relation.GetProperty();
            var required = await relation.IsRequired();
            var cardinality = await relation.GetCardinality();
            var permission = await relation.GetPermission();
            var provider = await GetProvider(relation.AttachmentID);
            return new AttachedRelationData(plainRelation, required, cardinality, permission, provider, relation.Conditions, relation.AttachmentID);
        }

        public async Task<PropertyProviderData> GetProvider(int id) {
            var provider = await storage.GetPropertyProvider(id);
            return await GetProvider(provider, id);
        }

        public async Task<PropertyProviderData> GetProvider(IPropertyProvider provider, int providerID) {
            var plainProviderAttributes = await provider.GetAttributes();
            var plainProviderRelations = await provider.GetRelations();
            var attributes = await Task.WhenAll(plainProviderAttributes.Select(attr => GetAttachedAttribute(attr)));
            var relations = await Task.WhenAll(plainProviderRelations.Select(relation => GetAttachedRelation(relation)));
            var attributesDict = attributes.ToDictionary(
                k => k.Attribute.ID,
                v => v
            );
            var relationsDict = relations.ToDictionary(
                k => k.Relation.PropertyID,
                v => v
            );
            return new PropertyProviderData(attributesDict, relationsDict, providerID);
        }


        public async Task<ModelData> GetModel(int id) {
            var model = await storage.GetModel(id);
            return await GetModel(model);
        }

        public async Task<ModelData> GetModel(IModel model) {
            var parent = await model.GetParentModel();
            var eventBase = await model.GetBaseEvent();
            var provider = await GetProvider(model.Properties, model.ModelID);

            return new ModelData {
                ID = model.ModelID,
                Label = model.Label,
                Parent = parent?.ModelID ?? 0,
                ParentLabel = parent?.Label,
                EventBase = eventBase.EventValue.ID,
                EventBaseName = eventBase.EventValue.Value,
                PropertyProvider = provider
            };
        }

        public async Task<int> AddAttribute(int providerID, PropertyAssignData data) {
            var eventID = await tx.AssignProviderAttribute(
                providerID,
                data.ID,
                data.Conditions ?? new ConditionRule.EventConditionRule(providerID)
            );

            if (Const.DefaultRequired != data.Required) {
                await tx.AssignPropertyValueRequirement(eventID, data.ID, data.Required);
            }

            if (Const.DefaultCardinality != data.Cardinality) {
                await tx.AssignPropertyValueCardinality(eventID, data.ID, data.Cardinality);
            }

            if (data.Permission.HasValue) {
                await tx.AssignPropertyValuePermission(eventID, data.ID, data.Permission.Value);
            }

            if (null != data.DefaultValue) {
                await tx.AssignPropertyValueSet(eventID, data.ID, data.DefaultValue.PlainValue);
            }

            return eventID;
        }

        public async Task<int> AddRelation(int providerID, PropertyAssignData data) {
            var eventID = await tx.AssignProviderRelation(providerID, data.ID);

            if (Const.DefaultRequired != data.Required) {
                await tx.AssignPropertyValueRequirement(eventID, data.ID, data.Required);
            }

            if (Const.DefaultCardinality != data.Cardinality) {
                await tx.AssignPropertyValueCardinality(eventID, data.ID, data.Cardinality);
            }

            if (data.Permission.HasValue) {
                await tx.AssignPropertyValuePermission(eventID, data.ID, data.Permission.Value);
            }

            return eventID;
        }

        public async Task<int> CreateModel(ModelCreateData model) {
            int modelID = await tx.CreateModel(model.EventBase.EventValue.ID, model.ParentModelID, model.Label);
            if (null != model.Properties) {
                await CreateScopedProperties(modelID, model.Properties);
            }
            return modelID;
        }

        private async Task CreateScopedProperties(
            int providerID,
            PropertyProviderData provider
        ) {
            foreach (var keyValue in provider.Attributes) {
                var property = keyValue.Value;
                var attachmentID = property.AttachmentID;

                if (!attachmentID.HasValue) {
                    attachmentID = await tx.AssignProviderAttribute(
                        providerID,
                        keyValue.Key,
                        keyValue.Value.Conditions ?? new ConditionRule.EventConditionRule(providerID)
                    );
                    await CreatePropertyConstraints(
                        attachmentID.Value,
                        property.Attribute.ID,
                        property.Required,
                        property.Cardinality,
                        property.DefaultValue
                    );
                }

                await CreateScopedProperties(attachmentID.Value, property.PropertyProvider);
            }

            foreach (var keyValue in provider.Relations) {
                var property = keyValue.Value;
                var attachmentID = property.AttachmentID;

                if (!attachmentID.HasValue) {
                    attachmentID = await tx.AssignProviderAttribute(
                        providerID,
                        keyValue.Key,
                        keyValue.Value.Conditions ?? new ConditionRule.EventConditionRule(providerID)
                    );
                    await CreatePropertyConstraints(
                        attachmentID.Value,
                        property.Relation.PropertyIndividual.IndividualID,
                        property.Required,
                        property.Cardinality,
                        null
                    );
                    continue;
                }

                await CreateScopedProperties(attachmentID.Value, property.PropertyProvider);
            }
        }

        private async Task CreatePropertyConstraints(int assignationID, int propertyID, bool required, int cardinality, IBoxedValue defaultValue) {
            if (Const.DefaultRequired != required) {
                await tx.AssignPropertyValueRequirement(assignationID, propertyID, required);
            }

            if (Const.DefaultCardinality != cardinality) {
                await tx.AssignPropertyValueCardinality(assignationID, propertyID, cardinality);
            }

            if (null != defaultValue) {
                await tx.AssignPropertyValueSet(assignationID, propertyID, defaultValue.PlainValue);
            }
        }
    }
}