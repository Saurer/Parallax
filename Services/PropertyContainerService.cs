using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuroraCore.Storage;
using Parallax.Models;

namespace Parallax.Services {
    public class PropertyContainerService {
        private readonly DialogService dialog;
        private readonly TransactionsService tx;
        private readonly IStorageAPI storage;

        public PropertyContainerService(DialogService dialog, TransactionsService tx, IStorageAPI storage) {
            this.dialog = dialog;
            this.tx = tx;
            this.storage = storage;
        }

        public async Task<int> CreateIndividual(IndividualCreateData model) {
            int eventID = await tx.CreateIndividual(model.BaseEvent, model.ModelID, model.Label);
            if (null != model.Properties) {
                await CreateScopedProperties(eventID, model.Properties.Attributes, container => container.Attributes);
                await CreateScopedProperties(eventID, model.Properties.Relations, container => container.Relations);
            }
            return eventID;
        }

        public async Task<PropertyContainerData> GetPropertyContainer(int containerID, IBoxedValue value = null) {
            var container = await storage.GetPropertyContainer(containerID);
            var plainAttributes = await container.GetAttributes();
            var plainRelations = await container.GetRelations();
            var resultAttributes = new Dictionary<int, IEnumerable<PropertyContainerData>>();
            var resultRelations = new Dictionary<int, IEnumerable<PropertyContainerData>>();
            var containerEvent = await storage.GetEvent(containerID);
            var actorEvent = await storage.GetEvent(containerEvent.EventValue.ActorEventID);

            foreach (var kv in plainAttributes) {
                var newValues = await Task.WhenAll(
                    kv.Value.Select(async v =>
                        await GetPropertyContainer(v.AssignationID, v)
                    )
                );
                resultAttributes.Add(kv.Key, newValues);
            }
            foreach (var kv in plainRelations) {
                var newValues = await Task.WhenAll(
                    kv.Value.Select(async v =>
                        await GetPropertyContainer(v.AssignationID, v)
                    )
                );
                resultRelations.Add(kv.Key, newValues);
            }

            return new PropertyContainerData(resultAttributes, resultRelations, containerID, actorEvent.EventValue.Value, value);
        }

        private async Task CreateScopedProperties(
            int containerID,
            IReadOnlyDictionary<int, IEnumerable<PropertyContainerData>> properties,
            Func<PropertyContainerData, IReadOnlyDictionary<int, IEnumerable<PropertyContainerData>>> scopeSelector
        ) {
            foreach (var property in properties) {
                foreach (var propertyValue in property.Value) {
                    var assignationID = await tx.AssignContainerProperty(containerID, property.Key, propertyValue.Value.PlainValue);
                    await CreateScopedProperties(assignationID, scopeSelector(propertyValue), scopeSelector);
                }
            }
        }
    }
}