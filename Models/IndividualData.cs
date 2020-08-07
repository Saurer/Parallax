using System.Collections.Generic;
using System.Threading.Tasks;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class IndividualData {
        public IIndividual Event { get; private set; }
        public int ID { get; private set; }
        public string Label { get; private set; }
        public IndividualModelData Model { get; private set; }
        public IReadOnlyDictionary<int, IndividualAttrData> Attributes { get; private set; }
        public IReadOnlyDictionary<int, IndividualRelationData> Relations { get; private set; }
        public int? Actor { get; private set; }
        public string ActorLabel { get; private set; }
        public bool Valid { get; private set; }

        private IndividualData() {

        }

        public static async Task<IndividualData> Instantiate(IIndividual individual) {
            var plainModel = await individual.GetModel();
            var plainModelAttributes = await plainModel.Properties.GetAttributes();
            var plainModelRelations = await plainModel.Properties.GetRelations();
            var attributes = await individual.Properties.GetAttributes();
            var relations = await individual.Properties.GetRelations();
            var model = await IndividualModelData.Instantiate(plainModel, attributes, relations);
            var valid = await individual.Properties.Validate();
            var attrValues = new Dictionary<int, IndividualAttrData>();
            var relationValues = new Dictionary<int, IndividualRelationData>();
            var actorEvent = await individual.GetCreator();

            foreach (var modelAttr in plainModelAttributes) {
                if (!attributes.ContainsKey(modelAttr.PropertyID)) {
                    continue;
                }

                var attrData = await IndividualAttrData.Instantiate(modelAttr, attributes[modelAttr.PropertyID]);
                attrValues.Add(modelAttr.PropertyID, attrData);
            }

            foreach (var modelRelation in plainModelRelations) {
                if (!relations.ContainsKey(modelRelation.PropertyID)) {
                    continue;
                }

                var relationData = await IndividualRelationData.Instantiate(modelRelation, relations[modelRelation.PropertyID]);
                relationValues.Add(modelRelation.PropertyID, relationData);
            }

            return new IndividualData {
                Event = individual,
                ID = individual.IndividualID,
                Label = individual.Label,
                Actor = actorEvent?.IndividualID,
                ActorLabel = actorEvent?.Label,
                Attributes = attrValues,
                Relations = relationValues,
                Model = model,
                Valid = valid
            };
        }
    }
}