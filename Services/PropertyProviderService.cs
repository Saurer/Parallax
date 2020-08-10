using System.Linq;
using System.Threading.Tasks;
using AuroraCore;
using Parallax.Models;

namespace Parallax.Services {
    public class PropertyProviderService {
        private readonly DialogService dialog;
        private readonly TransactionsService tx;

        public PropertyProviderService(DialogService dialog, TransactionsService tx) {
            this.dialog = dialog;
            this.tx = tx;
        }

        public async Task<bool> AddAttribute(PropertyProviderData provider) {
            var attributeIDs = provider.Attributes.Select(l => l.Attribute.ID);
            var result = await dialog.AddPropertyProviderAttribute(attributeIDs);

            if (null != result) {
                var eventID = await tx.AssignProviderAttribute(provider.ProviderID, result.ID);

                if (Const.DefaultRequired != result.Required) {
                    await tx.AssignPropertyValueRequirement(eventID, result.ID, result.Required);
                }

                if (Const.DefaultCardinality != result.Cardinality) {
                    await tx.AssignPropertyValueCardinality(eventID, result.ID, result.Cardinality);
                }

                return true;
            }

            return false;
        }

        public async Task<bool> AddRelation(PropertyProviderData provider) {
            var relationIDs = provider.Relations.Select(l => l.Relation.PropertyID);
            var result = await dialog.AddPropertyProviderRelation(relationIDs);

            if (null != result) {
                var eventID = await tx.AssignProviderRelation(provider.ProviderID, result.ID);

                if (Const.DefaultRequired != result.Required) {
                    await tx.AssignPropertyValueRequirement(eventID, result.ID, result.Required);
                }

                if (Const.DefaultCardinality != result.Cardinality) {
                    await tx.AssignPropertyValueCardinality(eventID, result.ID, result.Cardinality);
                }

                return true;
            }

            return false;
        }
    }
}