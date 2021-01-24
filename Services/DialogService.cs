// Parallax
// Copyright (C) 2020  Frank Horrigan <https://github.com/saurer>

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.Threading.Tasks;
using AuroraCore.Storage;
using Blazored.Modal;
using Blazored.Modal.Services;
using Parallax.Dialogs;
using Parallax.Models;

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

        public async Task<IBoxedValue> SetAttributeValue(AttachedAttrData attr, IBoxedValue value) {
            var parameters = new ModalParameters();
            parameters.Add("ModelAttr", attr);
            parameters.Add("Value", value);

            var modal = instance.Show<SetAttributeValue>("Set attribute", parameters);
            var result = await modal.Result;

            if (result.Data is IBoxedValue resultValue) {
                return resultValue;
            }
            else {
                return null;
            }
        }

        public async Task<IBoxedValue> SetRelationValue(AttachedRelationData relation, IBoxedValue value) {
            var parameters = new ModalParameters();
            parameters.Add("ModelRelation", relation);
            parameters.Add("Value", value);

            var modal = instance.Show<SetRelationValue>("Set relation", parameters);
            var result = await modal.Result;

            if (result.Data is IBoxedValue resultValue) {
                return resultValue;
            }
            else {
                return null;
            }
        }

        public async Task<PropertyAssignData> AddPropertyProviderAttribute(IEnumerable<int> availableAttributes) {
            var parameters = new ModalParameters();
            parameters.Add("Attributes", availableAttributes);
            var modal = instance.Show<AddModelAttribute>("Add attribute", parameters);
            var result = await modal.Result;

            if (result.Data is PropertyAssignData data) {
                return data;
            }
            else {
                return null;
            }
        }

        public async Task<PropertyAssignData> AddPropertyProviderRelation(IEnumerable<int> availableRelations) {
            var parameters = new ModalParameters();
            parameters.Add("Relations", availableRelations);
            var modal = instance.Show<AddModelRelation>("Add relation", parameters);
            var result = await modal.Result;

            if (result.Data is PropertyAssignData data) {
                return data;
            }
            else {
                return null;
            }
        }

        public async Task<EventAssignData> AddPropertyProviderEvent() {
            var parameters = new ModalParameters();
            // FIXME
            parameters.Add("Values", System.Array.Empty<int>());
            var modal = instance.Show<AddModelEvent>("Add event", parameters);
            var result = await modal.Result;

            if (result.Data is EventAssignData data) {
                return data;
            }
            else {
                return null;
            }
        }
    }
}