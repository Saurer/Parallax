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

using System.ComponentModel.DataAnnotations;

namespace Parallax.Models {
    public class IndividualCreateData {
        [Required, MinLength(1)]
        public string Label { get; set; }

        public int BaseEvent { get; private set; }
        public int ModelID { get; set; }
        public PropertyContainerData Properties { get; private set; }

        public IndividualCreateData(int baseEvent) {
            BaseEvent = baseEvent;
        }

        public void SetProperties(PropertyContainerData properties) {
            Properties = properties;
        }

        public void ResetProperties() {
            if (null == Properties) {
                return;
            }

            Properties.Clear();
        }
    }
}