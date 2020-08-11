namespace Parallax.Models {
    public class AttachedAttrData {
        public int? AttachmentID { get; private set; }
        public AttrData Attribute { get; private set; }
        public bool Required { get; private set; }
        public int Cardinality { get; private set; }
        public int? Permission { get; private set; }
        public PropertyProviderData PropertyProvider { get; private set; }

        public AttachedAttrData(
            AttrData attr,
            bool required,
            int cardinality,
            int? permission
        ) : this(attr, required, cardinality, permission, null) {
            PropertyProvider = new PropertyProviderData();
            AttachmentID = null;
        }

        public AttachedAttrData(
            AttrData attr,
            bool required,
            int cardinality,
            int? permission,
            PropertyProviderData provider,
            int attachmentID
        ) : this(attr, required, cardinality, permission, provider) {
            AttachmentID = attachmentID;
        }

        private AttachedAttrData(
            AttrData attr,
            bool required,
            int cardinality,
            int? permission,
            PropertyProviderData provider
        ) {
            Attribute = attr;
            Required = required;
            Cardinality = cardinality;
            Permission = permission;
            PropertyProvider = provider;
        }
    }
}