using System.Linq;
using System.Collections.Generic;
using AuroraCore.Storage;
using System.Threading.Tasks;

namespace Parallax.Models {
    public class ModelData {
        public int ID { get; private set; }
        public string Label { get; private set; }
        public int Parent { get; private set; }
        public string ParentLabel { get; private set; }
        public int EventBase { get; private set; }
        public string EventBaseName { get; private set; }
        public IEnumerable<ModelAttrData> Attributes { get; private set; }
        public IEnumerable<ModelRelationData> Relations { get; private set; }

        private ModelData() { }

        public static async Task<ModelData> Instantiate(IModel model) {
            var parent = await model.GetParentModel();
            var eventBase = await model.GetBaseEvent();
            var plainModelAttributes = await model.Properties.GetAttributes();
            var attributes = await Task.WhenAll(plainModelAttributes.Select(attr => ModelAttrData.Instantiate(attr)));
            var plainModelRelations = await model.Properties.GetRelations();
            var relations = await Task.WhenAll(plainModelRelations.Select(relation => ModelRelationData.Instantiate(relation)));

            return new ModelData() {
                ID = model.ModelID,
                Label = model.Label,
                Parent = parent?.ModelID ?? 0,
                ParentLabel = parent?.Label,
                EventBase = eventBase.EventValue.ID,
                EventBaseName = eventBase.EventValue.Value,
                Attributes = attributes,
                Relations = relations
            };
        }
    }
}