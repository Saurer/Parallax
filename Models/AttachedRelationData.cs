using AuroraCore.Storage;

namespace Parallax.Models {
    public class AttachedRelationData {
        public int? AttachmentID { get; private set; }
        public IRelation Relation { get; private set; }
        public bool Required { get; private set; }
        public bool Mutable { get; private set; }
        public int Cardinality { get; private set; }
        public int? Permission { get; private set; }
        public ConditionRule Conditions { get; private set; }
        public PropertyProviderData PropertyProvider { get; private set; }

        public AttachedRelationData(
            IRelation relation,
            bool required,
            int cardinality,
            bool mutable,
            int? permission,
            ConditionRule conditions
        ) : this(relation, required, cardinality, mutable, permission, conditions, null) {
            PropertyProvider = new PropertyProviderData();
        }

        public AttachedRelationData(
            IRelation relation,
            bool required,
            int cardinality,
            bool mutable,
            int? permission,
            PropertyProviderData provider,
            ConditionRule conditions,
            int attachmentID
        ) : this(relation, required, cardinality, mutable, permission, conditions, provider) {
            AttachmentID = attachmentID;
        }

        private AttachedRelationData(
            IRelation relation,
            bool required,
            int cardinality,
            bool mutable,
            int? permission,
            ConditionRule conditions,
            PropertyProviderData provider
        ) {
            Relation = relation;
            Required = required;
            Mutable = mutable;
            Cardinality = cardinality;
            Permission = permission;
            Conditions = conditions;
            PropertyProvider = provider;
        }
    }
}