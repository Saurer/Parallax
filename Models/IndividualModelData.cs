using System.Linq;
using System.Collections.Generic;
using AuroraCore.Storage;
using System.Threading.Tasks;

namespace Parallax.Models {
    public class IndividualModelData {
        public int ID { get; private set; }
        public string Label { get; private set; }
        public int Parent { get; private set; }
        public IModel Value { get; private set; }
        public IEnumerable<IndividualAttrData> Attributes { get; private set; }
        public IEnumerable<IndividualRelationData> Relations { get; private set; }

        private IndividualModelData() {

        }

        public static async Task<IndividualModelData> Instantiate(IModel model, IReadOnlyDictionary<int, IEnumerable<IBoxedValue>> attributeValues, IReadOnlyDictionary<int, IEnumerable<IBoxedValue>> relationValues) {
            var parent = await model.GetParentModel();
            var plainModelAttributes = await model.Properties.GetAttributes();
            var attributes = await Task.WhenAll(plainModelAttributes.Select(modelAttr =>
                IndividualAttrData.Instantiate(
                    modelAttr,
                    attributeValues.ContainsKey(modelAttr.PropertyID) ? attributeValues[modelAttr.PropertyID] : null
                )
            ));
            var plainModelRelations = await model.Properties.GetRelations();
            var relations = await Task.WhenAll(plainModelRelations.Select(modelRelation =>
                IndividualRelationData.Instantiate(
                    modelRelation,
                    relationValues.ContainsKey(modelRelation.PropertyID) ? relationValues[modelRelation.PropertyID] : null
                )
            ));

            return new IndividualModelData {
                ID = model.ModelID,
                Label = model.Label,
                Parent = parent?.ModelID ?? 0,
                Value = model,
                Attributes = attributes,
                Relations = relations
            };
        }
    }
}