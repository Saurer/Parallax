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
        public IReadOnlyDictionary<int, IndividualRelationData> Relations { get; private set; }
        public int? Actor { get; private set; }
        public string ActorName { get; private set; }
        public bool Valid { get; private set; }

        private IndividualData() {

        }

        public static async Task<IndividualData> Instantiate(IIndividual individual) {
            var plainModel = await individual.GetModel();
            var plainModelAttributes = await plainModel.GetAllAttributes();
            var plainModelRelations = await plainModel.GetAllRelations();
            var attributes = await individual.GetAttributes();
            var relations = await individual.GetRelations();
            var model = await IndividualModelData.Instantiate(plainModel, attributes, relations);
            var valid = await plainModel.Validate(attributes, relations);
            var actorEvent = await individual.GetActor();
            var attrValues = new Dictionary<int, IndividualAttrData>();
            var relationValues = new Dictionary<int, IndividualRelationData>();

            foreach (var modelAttr in plainModelAttributes) {
                int attrID = Int32.Parse(modelAttr.Value);

                if (!attributes.ContainsKey(attrID)) {
                    continue;
                }

                var attrData = await IndividualAttrData.Instantiate(modelAttr, attributes[attrID]);
                attrValues.Add(attrID, attrData);
            }

            foreach (var modelRelation in plainModelRelations) {
                int relationID = Int32.Parse(modelRelation.Value);

                if (!relations.ContainsKey(relationID)) {
                    continue;
                }

                var relationData = await IndividualRelationData.Instantiate(modelRelation, relations[relationID]);
                relationValues.Add(relationID, relationData);
            }

            return new IndividualData {
                Event = individual,
                ID = individual.ID,
                Name = individual.Value,
                Actor = actorEvent?.ID,
                ActorName = actorEvent?.Value,
                Attributes = attrValues,
                Relations = relationValues,
                Model = model,
                Valid = valid
            };
        }
    }
}