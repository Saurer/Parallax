namespace Parallax.Models {
    public class AttachedAttrData {
        public int? AttachmentID { get; private set; }
        public AttrData Attribute { get; private set; }
        public bool Required { get; private set; }
        public int Cardinality { get; private set; }
        public PropertyProviderData PropertyProvider { get; private set; }

        public AttachedAttrData(
            AttrData attr,
            bool required,
            int cardinality
        ) : this(attr, required, cardinality, null) {
            PropertyProvider = new PropertyProviderData();
            AttachmentID = null;
        }

        public AttachedAttrData(
            AttrData attr,
            bool required,
            int cardinality,
            PropertyProviderData provider,
            int attachmentID
        ) : this(attr, required, cardinality, provider) {
            AttachmentID = attachmentID;
        }

        private AttachedAttrData(
            AttrData attr,
            bool required,
            int cardinality,
            PropertyProviderData provider
        ) {
            Attribute = attr;
            Required = required;
            Cardinality = cardinality;
            PropertyProvider = provider;
        }
    }
}