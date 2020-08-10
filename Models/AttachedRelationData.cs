using AuroraCore.Storage;
using System.Threading.Tasks;

namespace Parallax.Models {
    public class AttachedRelationData {
        public int AttachmentID { get; private set; }
        public IRelation Relation { get; private set; }
        public bool Required { get; private set; }
        public int Cardinality { get; private set; }
        public PropertyProviderData PropertyProvider { get; private set; }

        private AttachedRelationData() {

        }

        public static async Task<AttachedRelationData> Instantiate(IAttachedProperty<IRelation> relation) {
            var relationIndividual = await relation.GetProperty();
            var required = await relation.IsRequired();
            var cardinality = await relation.GetCardinality();
            var provider = await PropertyProviderData.Instantiate(relation.AttachmentID, relation.Properties);

            return new AttachedRelationData {
                AttachmentID = relation.AttachmentID,
                Required = required,
                Cardinality = cardinality,
                Relation = relationIndividual,
                PropertyProvider = provider
            };
        }
    }
}