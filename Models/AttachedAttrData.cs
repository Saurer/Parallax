using AuroraCore.Storage;
using System.Threading.Tasks;

namespace Parallax.Models {
    public class AttachedAttrData {
        public AttrData Attribute { get; private set; }
        public bool Required { get; private set; }
        public int Cardinality { get; private set; }
        public PropertyProviderData PropertyProvider { get; private set; }

        private AttachedAttrData() {

        }

        public static async Task<AttachedAttrData> Instantiate(IAttachedProperty<IAttr> attr) {
            var plainAttr = await attr.GetProperty();
            var attrData = await AttrData.Instantiate(plainAttr);
            var required = await attr.IsRequired();
            var cardinality = await attr.GetCardinality();
            var provider = await PropertyProviderData.Instantiate(attr.AttachmentID, attr.Properties);

            return new AttachedAttrData {
                Required = required,
                Cardinality = cardinality,
                Attribute = attrData,
                PropertyProvider = provider
            };
        }
    }
}