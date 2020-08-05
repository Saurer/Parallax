using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuroraCore;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class IndividualRelationData {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public IModelProperty<IRelation> ModelRelation { get; private set; }
        public IEnumerable<string> PlainValues { get; private set; }
        public IEnumerable<string> ProcessedValues { get; private set; }
        public IReadOnlyDictionary<int, string> ValueProperties { get; private set; }
        public bool Required {
            get {
                if (ValueProperties.ContainsKey(StaticEvent.Required)) {
                    return ValueProperties[StaticEvent.Required] == "1";
                }
                else {
                    return Const.DefaultRequired == 1;
                }
            }
        }
        public int Cardinality {
            get {
                if (ValueProperties.ContainsKey(StaticEvent.Cardinality)) {
                    return Int32.Parse(ValueProperties[StaticEvent.Cardinality]);
                }
                else {
                    return Const.DefaultCardinality;
                }
            }
        }

        private IndividualRelationData() {

        }

        public static async Task<IndividualRelationData> Instantiate(IModelProperty<IRelation> modelRelation, IEnumerable<string> values = null) {
            var relation = await modelRelation.GetProperty();
            var plainValueProperties = await modelRelation.GetValueProperties();
            var valueProperties = new Dictionary<int, string>();
            // IEnumerable<string> processedValues;

            foreach (var prop in plainValueProperties) {
                valueProperties.Add(prop.ValueID, prop.Value);
            }

            return new IndividualRelationData {
                ID = relation.ID,
                Name = relation.Value,
                ModelRelation = modelRelation,
                PlainValues = values,
                ProcessedValues = values,
                ValueProperties = valueProperties
            };
        }
    }
}