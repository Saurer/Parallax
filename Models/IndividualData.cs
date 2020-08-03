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
        public int? Actor { get; private set; }
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
            }

            return new IndividualData {
                Event = individual,
                ID = individual.ID,
                Name = individual.Value,
                Actor = actorEvent?.ID,
                ActorName = actorEvent?.Value,
                Attributes = attrValues,
                Model = model,
                Valid = valid
            };
        }
    }
}