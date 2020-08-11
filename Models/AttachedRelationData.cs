using AuroraCore.Storage;

namespace Parallax.Models {
    public class AttachedRelationData {
        public int? AttachmentID { get; private set; }
        public IRelation Relation { get; private set; }
        public bool Required { get; private set; }
        public int Cardinality { get; private set; }
        public int? Permission { get; private set; }
        public PropertyProviderData PropertyProvider { get; private set; }

        public AttachedRelationData(
            IRelation relation,
            bool required,
            int cardinality,
            int? permission
        ) : this(relation, required, cardinality, permission, null) {
            PropertyProvider = new PropertyProviderData();
        }

        public AttachedRelationData(
            IRelation relation,
            bool required,
            int cardinality,
            int? permission,
            PropertyProviderData provider,
            int attachmentID
        ) : this(relation, required, cardinality, permission, provider) {
            AttachmentID = attachmentID;
        }

        private AttachedRelationData(
            IRelation relation,
            bool required,
            int cardinality,
            int? permission,
            PropertyProviderData provider
        ) {
            Relation = relation;
            Required = required;
            Cardinality = cardinality;
            Permission = permission;
            PropertyProvider = provider;
        }
    }
}