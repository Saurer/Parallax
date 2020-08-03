using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class IndividualData {
        public IIndividual Event { get; private set; }
        public int ID { get; private set; }
        public string Name { get; private set; }
        public IndividualModelData Model { get; private set; }
        public IReadOnlyDictionary<int, IndividualAttrData> Attributes { get; private set; }
        public string ActorName { get; private set; }
        public bool Valid { get; private set; }

        private IndividualData() {

        }

        public static async Task<IndividualData> Instantiate(IIndividual individual) {
            var plainModel = await individual.GetModel();
            var plainModelAttributes = await plainModel.GetAllAttributes();
            var attributes = await individual.GetAttributes();
            var model = await IndividualModelData.Instantiate(plainModel, attributes);
            var valid = await plainModel.Validate(attributes);
            var actorEvent = await individual.GetActor();
            var attrValues = new Dictionary<int, IndividualAttrData>();

            foreach (var modelAttr in plainModelAttributes) {
                int attrID = Int32.Parse(modelAttr.Value);

                if (!attributes.ContainsKey(attrID)) {
                    continue;
                }

                var attrData = await IndividualAttrData.Instantiate(modelAttr, attributes[attrID]);
                attrValues.Add(attrID, attrData);

                // var values = attributes[attr.ID];
                // var boxed = await attr.IsBoxed();
                // foreach (var value in values) {
                //     if (!attrValues.ContainsKey(attr.ID)) {
                //         attrValues.Add(attr.ID, new List<string>());
                //     }

                //     var list = (List<string>)attrValues[attr.ID];

                //     if (boxed) {
                //         var valueEvent = await attr.GetValue(Int32.Parse(value));
                //         list.Add(valueEvent.Value);
                //     }
                //     else {
                //         list.Add(value);
                //     }
                // }
            }

            return new IndividualData {
                Event = individual,
                ID = individual.ID,
                Name = individual.Value,
                ActorName = actorEvent?.Value,
                Attributes = attrValues,
                Model = model,
                Valid = valid
            };
        }
    }
}