using System.Linq;
using System.Collections.Generic;
using AuroraCore.Storage;
using System.Threading.Tasks;
using System;

namespace Parallax.Models {
    public class ModelData {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int Parent { get; private set; }
        public string ParentName { get; private set; }
        public int EventBase { get; private set; }
        public string EventBaseName { get; private set; }
        public IEnumerable<ModelAttrData> OwnAttributes { get; private set; }
        public IEnumerable<ModelAttrData> InheritedAttributes { get; private set; }
        public IEnumerable<ModelAttrData> AllAttributes {
            get {
                foreach (var attr in InheritedAttributes) {
                    yield return attr;
                }

                foreach (var attr in OwnAttributes) {
                    yield return attr;
                }
            }
        }

        private ModelData() { }

        public static async Task<ModelData> Instantiate(IModel model) {
            var parent = await model.GetParent();
            var eventBase = await model.GetBaseEvent();
            var plainModelAttributes = await model.GetOwnAttributes();
            var ownAttributes = await Task.WhenAll(plainModelAttributes.Select(attr => ModelAttrData.Instantiate(attr)));
            var inheritedAttributes = Array.Empty<ModelAttrData>();

            if (null != parent) {
                var plainInheritedModelAttributes = await parent.GetAllAttributes();
                inheritedAttributes = await Task.WhenAll(plainInheritedModelAttributes.Select(attr => ModelAttrData.Instantiate(attr)));
            }

            return new ModelData() {
                ID = model.ID,
                Name = model.Value,
                Parent = parent?.ID ?? 0,
                ParentName = parent?.Value,
                EventBase = eventBase.ID,
                EventBaseName = eventBase.Value,
                OwnAttributes = ownAttributes,
                InheritedAttributes = inheritedAttributes
            };
        }
    }
}