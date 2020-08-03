using System.Linq;
using System.Collections.Generic;
using AuroraCore.Storage;
using System.Threading.Tasks;
using System;

namespace Parallax.Models {
    public class IndividualModelData {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int Parent { get; private set; }
        public IEnumerable<IndividualAttrData> Attributes { get; private set; }

        private IndividualModelData() {

        }

        public static async Task<IndividualModelData> Instantiate(IModel model, IReadOnlyDictionary<int, IEnumerable<string>> values) {
            var parent = await model.GetParent();
            var plainModelAttributes = await model.GetAllAttributes();
            var attributes = await Task.WhenAll(plainModelAttributes.Select(modelAttr => {
                var attrID = Int32.Parse(modelAttr.Value);
                return IndividualAttrData.Instantiate(
                    modelAttr,
                    values.ContainsKey(attrID) ? values[attrID] : null
                );
            }));

            return new IndividualModelData {
                ID = model.ID,
                Name = model.Value,
                Parent = parent?.ID ?? 0,
                Attributes = attributes
            };
        }
    }
}