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
        public IModel Value { get; private set; }
        public IEnumerable<IndividualAttrData> Attributes { get; private set; }
        public IEnumerable<IndividualRelationData> Relations { get; private set; }

        private IndividualModelData() {

        }

        public static async Task<IndividualModelData> Instantiate(IModel model, IReadOnlyDictionary<int, IEnumerable<string>> attributeValues, IReadOnlyDictionary<int, IEnumerable<string>> relationValues) {
            var parent = await model.GetParent();
            var plainModelAttributes = await model.GetAllAttributes();
            var attributes = await Task.WhenAll(plainModelAttributes.Select(modelAttr => {
                var attrID = Int32.Parse(modelAttr.Value);
                return IndividualAttrData.Instantiate(
                    modelAttr,
                    attributeValues.ContainsKey(attrID) ? attributeValues[attrID] : null
                );
            }));
            var plainModelRelations = await model.GetAllRelations();
            var relations = await Task.WhenAll(plainModelRelations.Select(modelRelation => {
                var relationID = Int32.Parse(modelRelation.Value);
                return IndividualRelationData.Instantiate(
                    modelRelation,
                    relationValues.ContainsKey(relationID) ? relationValues[relationID] : null
                );
            }));

            return new IndividualModelData {
                ID = model.ID,
                Name = model.Value,
                Parent = parent?.ID ?? 0,
                Value = model,
                Attributes = attributes,
                Relations = relations
            };
        }
    }
}