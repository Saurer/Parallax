using System.Linq;
using System.Collections.Generic;
using AuroraCore.Storage;
using System.Threading.Tasks;

namespace Parallax.Models {
    public class ModelData {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int Parent { get; private set; }
        public string ParentName { get; private set; }
        public int EventBase { get; private set; }
        public string EventBaseName { get; private set; }
        public IEnumerable<ModelAttrData> Attributes { get; private set; }
        public IEnumerable<ModelRelationData> Relations { get; private set; }

        private ModelData() { }

        public static async Task<ModelData> Instantiate(IModel model) {
            var parent = await model.GetParent();
            var eventBase = await model.GetBaseEvent();
            var plainModelAttributes = await model.GetAllAttributes();
            var attributes = await Task.WhenAll(plainModelAttributes.Select(attr => ModelAttrData.Instantiate(attr)));
            var plainModelRelations = await model.GetAllRelations();
            var relations = await Task.WhenAll(plainModelRelations.Select(relation => ModelRelationData.Instantiate(relation)));

            return new ModelData() {
                ID = model.ID,
                Name = model.Value,
                Parent = parent?.ID ?? 0,
                ParentName = parent?.Value,
                EventBase = eventBase.ID,
                EventBaseName = eventBase.Value,
                Attributes = attributes,
                Relations = relations
            };
        }
    }
}