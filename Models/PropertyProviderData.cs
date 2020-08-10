using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class PropertyProviderData {
        public int ProviderID { get; private set; }
        public IEnumerable<AttachedAttrData> Attributes { get; private set; }
        public IEnumerable<AttachedRelationData> Relations { get; private set; }

        public static async Task<PropertyProviderData> Instantiate(int id, IPropertyProvider provider) {
            var plainProviderAttributes = await provider.GetAttributes();
            var attributes = await Task.WhenAll(plainProviderAttributes.Select(attr => AttachedAttrData.Instantiate(attr)));
            var plainProviderRelations = await provider.GetRelations();
            var relations = await Task.WhenAll(plainProviderRelations.Select(relation => AttachedRelationData.Instantiate(relation)));

            return new PropertyProviderData {
                ProviderID = id,
                Attributes = attributes,
                Relations = relations
            };
        }
    }
}