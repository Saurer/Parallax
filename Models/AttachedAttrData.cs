using AuroraCore.Storage;

namespace Parallax.Models {
    public class AttachedAttrData {
        public int? AttachmentID { get; private set; }
        public AttrData Attribute { get; private set; }
        public bool Required { get; private set; }
        public int Cardinality { get; private set; }
        public int? Permission { get; private set; }
        public ConditionRule Conditions { get; private set; }
        public PropertyProviderData PropertyProvider { get; private set; }

        public AttachedAttrData(
            AttrData attr,
            bool required,
            int cardinality,
            int? permission,
            ConditionRule conditions
        ) : this(attr, required, cardinality, permission, conditions, null) {
            PropertyProvider = new PropertyProviderData();
            AttachmentID = null;
        }

        public AttachedAttrData(
            AttrData attr,
            bool required,
            int cardinality,
            int? permission,
            PropertyProviderData provider,
            ConditionRule conditions,
            int attachmentID
        ) : this(attr, required, cardinality, permission, conditions, provider) {
            AttachmentID = attachmentID;
        }

        private AttachedAttrData(
            AttrData attr,
            bool required,
            int cardinality,
            int? permission,
            ConditionRule conditions,
            PropertyProviderData provider
        ) {
            Attribute = attr;
            Required = required;
            Cardinality = cardinality;
            Permission = permission;
            Conditions = conditions;
            PropertyProvider = provider;
        }
    }
}