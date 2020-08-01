using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class IndividualData {
        public IIndividual Event { get; private set; }
        public int ID { get; private set; }
        public string Name { get; private set; }
        public IndividualModelData Model { get; private set; }
        public IReadOnlyDictionary<int, string> Attributes { get; private set; }
        public bool Valid { get; private set; }

        private IndividualData() {

        }

        public static async Task<IndividualData> Instantiate(IIndividual individual) {
            var plainModel = await individual.GetModel();
            var plainModelAttributes = await plainModel.GetAllAttributes();
            var plainAttributes = await Task.WhenAll(plainModelAttributes.Select(attr => attr.GetAttribute()));
            var attributes = await individual.GetAttributes();
            var model = await IndividualModelData.Instantiate(plainModel, attributes);
            var valid = await plainModel.Validate(attributes);
            var attrValues = new Dictionary<int, string>();

            foreach (var attr in plainAttributes) {
                if (!attributes.ContainsKey(attr.ID)) {
                    continue;
                }

                var value = attributes[attr.ID];
                var boxed = await attr.IsBoxed();
                if (boxed) {
                    var valueEvent = await attr.GetValue(Int32.Parse(value));
                    attrValues.Add(attr.ID, valueEvent.Value);
                }
                else {
                    attrValues.Add(attr.ID, value);
                }
            }

            return new IndividualData {
                Event = individual,
                ID = individual.ID,
                Name = individual.Value,
                Attributes = attrValues,
                Model = model,
                Valid = valid
            };
        }
    }
}