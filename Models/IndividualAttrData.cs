using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuroraCore;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class IndividualAttrData {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public IModelAttr ModelAttr { get; private set; }
        public IEnumerable<string> PlainValues { get; private set; }
        public IEnumerable<string> ProcessedValues { get; private set; }
        public IEnumerable<AttrPropertyData> AttrProperties { get; private set; }
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

        private IndividualAttrData() {

        }

        public static async Task<IndividualAttrData> Instantiate(IModelAttr modelAttr, IEnumerable<string> values = null) {
            var attr = await modelAttr.GetAttribute();
            var plainProperties = await attr.GetProperties();
            var dataType = await attr.GetDataType();
            var attrProperties =
                from p in plainProperties
                select new AttrPropertyData(p.BaseEventID, p.ValueID, Int32.Parse(p.Value));
            var plainValueProperties = await modelAttr.GetValueProperties();
            var valueProperties = new Dictionary<int, string>();
            IEnumerable<string> processedValues;

            foreach (var prop in plainValueProperties) {
                valueProperties.Add(prop.ValueID, prop.Value);
            }

            if (dataType.IsBoxed && null != values) {
                var boxedValues = await attr.GetValues();
                processedValues = values.Select(v =>
                    boxedValues
                        .Where(b => b.ID.ToString() == v)
                        .Select(r => r.Value)
                        .SingleOrDefault()
                );
            }
            else {
                processedValues = values;
            }

            return new IndividualAttrData {
                ID = attr.ID,
                Name = attr.Value,
                Type = dataType.Name,
                ModelAttr = modelAttr,
                PlainValues = values,
                ProcessedValues = processedValues,
                AttrProperties = attrProperties,
                ValueProperties = valueProperties
            };
        }
    }
}