using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class IndividualAttrData {
        public int ID { get; private set; }
        public string Label { get; private set; }
        public string Type { get; private set; }
        public IAttachedProperty<IAttr> AttachedProperty { get; private set; }
        public IEnumerable<IBoxedValue> Values { get; private set; }
        public bool Required { get; private set; }
        public int Cardinality { get; private set; }

        private IndividualAttrData() {

        }

        public static async Task<IndividualAttrData> Instantiate(IAttachedProperty<IAttr> attachedProperty, IEnumerable<IBoxedValue> values = null) {
            var attr = await attachedProperty.GetProperty();
            var dataType = await attr.GetDataType();
            var required = await attachedProperty.IsRequired();
            var cardinality = await attachedProperty.GetCardinality();
            var plainValues = await attr.GetValueCandidates();
            var processedValues = plainValues.Select(v => v.EventValue.Value);

            return new IndividualAttrData {
                ID = attr.PropertyID,
                Label = attr.Label,
                Type = dataType.Name,
                AttachedProperty = attachedProperty,
                Values = values,
                Required = required,
                Cardinality = cardinality
            };
        }
    }
}