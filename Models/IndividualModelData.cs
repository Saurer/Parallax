using System.Linq;
using System.Collections.Generic;
using AuroraCore.Storage;
using System.Threading.Tasks;

namespace Parallax.Models {
    public class IndividualModelData {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int Parent { get; private set; }
        public IEnumerable<IndividualAttrData> Attributes { get; private set; }

        private IndividualModelData() {

        }

        public static async Task<IndividualModelData> Instantiate(IModel model, IReadOnlyDictionary<int, string> values) {
            var parent = await model.GetParent();
            var plainModelAttributes = await model.GetAllAttributes();
            var plainAttributes = await Task.WhenAll(plainModelAttributes.Select(attr => attr.GetAttribute()));
            var attributes = await Task.WhenAll(plainAttributes.Select(a =>
                IndividualAttrData.Instantiate(a, values.ContainsKey(a.ID) ? values[a.ID] : null)
            ));

            return new IndividualModelData {
                ID = model.ID,
                Name = model.Value,
                Parent = parent?.ID ?? 0,
                Attributes = attributes
            };
        }
    }
}