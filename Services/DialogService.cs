using System.Collections.Generic;
using System.Threading.Tasks;
using AuroraCore.Storage;
using Blazored.Modal;
using Blazored.Modal.Services;
using Parallax.Dialogs;

namespace Parallax.Services {
    public class DialogService {
        private IModalService instance;

        public DialogService(IModalService instance) {
            this.instance = instance;
        }

        public async Task Error(string text) {
            var parameters = new ModalParameters();
            parameters.Add("Text", "Provided file contains invalid data");
            var modal = instance.Show<Alert>("Error", parameters);
            await modal.Result;
        }

        public async Task<bool> ResetGraph() {
            var modal = instance.Show<ResetGraph>("Confirmation");
            var result = await modal.Result;
            return result.Data is true;
        }

        public async Task<bool> Confirm(string text) {
            var parameters = new ModalParameters();
            parameters.Add("Text", text);
            var modal = instance.Show<Confirmation>("Confirmation", parameters);
            var modalResult = await modal.Result;
            return modalResult.Data is true;
        }

        public async Task<bool> AssignIndividualAttribute(IIndividual individual, int attrID) {
            var parameters = new ModalParameters();
            parameters.Add("EventID", individual.ID);
            parameters.Add("AttrID", attrID);

            var modal = instance.Show<AssignAttribute>($"Assign attribute to '{individual.Value}'", parameters);
            var result = await modal.Result;
            return result.Data is true;
        }

        public async Task<string> AddAttributeValue(int attributeID) {
            var parameters = new ModalParameters();
            parameters.Add("ID", attributeID);

            var modal = instance.Show<AddAttributeValue>("Add attribute value", parameters);
            var result = await modal.Result;

            if (!result.Cancelled && result.Data is string strResult) {
                return strResult;
            }
            else {
                return null;
            }
        }

        public async Task<string> SetAttributeValue(int attributeID, string value) {
            var parameters = new ModalParameters();
            parameters.Add("AttrID", attributeID);
            parameters.Add("Value", value);

            var modal = instance.Show<SetAttributeValue>("Set attribute", parameters);
            var result = await modal.Result;

            if (result.Data is string strValue) {
                return strValue;
            }
            else {
                return null;
            }
        }

        public async Task<int?> CreateModel(int baseEvent, int parentModel) {
            var parameters = new ModalParameters();
            parameters.Add("BaseEventID", baseEvent);
            parameters.Add("ParentModelID", parentModel);
            var modal = instance.Show<CreateModel>("Create model", parameters);
            var result = await modal.Result;

            if (result.Data is int intValue) {
                return intValue;
            }
            else {
                return null;
            }
        }

        public async Task<int?> AddModelAttribute(IEnumerable<int> availableAttributes) {
            var parameters = new ModalParameters();
            parameters.Add("Attributes", availableAttributes);
            var modal = instance.Show<AddModelAttribute>("Add attribute", parameters);
            var result = await modal.Result;

            if (result.Data is int intValue) {
                return intValue;
            }
            else {
                return null;
            }
        }
    }
}