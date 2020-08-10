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
        public PropertyProviderData PropertyProvider { get; private set; }

        private ModelData() { }

        public static async Task<ModelData> Instantiate(IModel model) {
            var parent = await model.GetParentModel();
            var eventBase = await model.GetBaseEvent();
            var provider = await PropertyProviderData.Instantiate(model.ModelID, model.Properties);

            return new ModelData() {
                ID = model.ModelID,
                Label = model.Label,
                Parent = parent?.ModelID ?? 0,
                ParentLabel = parent?.Label,
                EventBase = eventBase.EventValue.ID,
                EventBaseName = eventBase.EventValue.Value,
                PropertyProvider = provider
            };
        }
    }
}