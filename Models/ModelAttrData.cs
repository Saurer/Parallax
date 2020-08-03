using AuroraCore.Storage;
using System.Threading.Tasks;

namespace Parallax.Models {
    public class ModelAttrData {
        public int ID { get; private set; }
        public AttrData Attribute { get; private set; }
        public bool Required { get; private set; }
        public int Cardinality { get; private set; }

        private ModelAttrData() {

        }

        public static async Task<ModelAttrData> Instantiate(IModelAttr attr) {
            var plainAttr = await attr.GetAttribute();
            var attrData = await AttrData.Instantiate(plainAttr);
            var required = await attr.IsRequired();
            var cardinality = await attr.GetCardinality();

            return new ModelAttrData {
                ID = attr.ID,
                Required = required,
                Cardinality = cardinality,
                Attribute = attrData
            };
        }
    }
}