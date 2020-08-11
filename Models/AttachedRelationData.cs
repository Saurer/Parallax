using AuroraCore.Storage;

namespace Parallax.Models {
    public class AttachedRelationData {
        public int? AttachmentID { get; private set; }
        public IRelation Relation { get; private set; }
        public bool Required { get; private set; }
        public int Cardinality { get; private set; }
        public PropertyProviderData PropertyProvider { get; private set; }

        public AttachedRelationData(
            IRelation relation,
            bool required,
            int cardinality
        ) : this(relation, required, cardinality, null) {
            PropertyProvider = new PropertyProviderData();
        }

        public AttachedRelationData(
            IRelation relation,
            bool required,
            int cardinality,
            PropertyProviderData provider,
            int attachmentID
        ) : this(relation, required, cardinality, provider) {
            AttachmentID = attachmentID;
        }

        private AttachedRelationData(
            IRelation relation,
            bool required,
            int cardinality,
            PropertyProviderData provider
        ) {
            Relation = relation;
            Required = required;
            Cardinality = cardinality;
            PropertyProvider = provider;
        }
    }
}