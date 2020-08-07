using System.Collections.Generic;
using System.Threading.Tasks;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class IndividualRelationData {
        public int ID { get; private set; }
        public string Label { get; private set; }
        public IAttachedProperty<IRelation> AttachedProperty { get; private set; }
        public IEnumerable<IBoxedValue> Values { get; private set; }
        public bool Required { get; private set; }
        public int Cardinality { get; private set; }

        private IndividualRelationData() {

        }

        public static async Task<IndividualRelationData> Instantiate(IAttachedProperty<IRelation> attachedProperty, IEnumerable<IBoxedValue> values = null) {
            var relation = await attachedProperty.GetProperty();
            var required = await attachedProperty.IsRequired();
            var cardinality = await attachedProperty.GetCardinality();

            return new IndividualRelationData {
                ID = relation.PropertyID,
                Label = relation.Label,
                AttachedProperty = attachedProperty,
                Values = values,
                Required = required,
                Cardinality = cardinality
            };
        }
    }
}