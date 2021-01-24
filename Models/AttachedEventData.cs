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

using AuroraCore.Storage;

namespace Parallax.Models {
    public class AttachedEventData {
        public int? AttachmentID { get; private set; }
        public IProperty Property { get; private set; }
        public string Value { get; private set; }
        public ConditionRule Conditions { get; private set; }

        public AttachedEventData(
            IProperty property,
            ConditionRule conditions,
            string value,
            int attachmentID
        ) : this(property, conditions, value) {
            AttachmentID = attachmentID;
        }

        public AttachedEventData(
            IProperty property,
            ConditionRule conditions,
            string value
        ) {
            Property = property;
            Conditions = conditions;
            Value = value;
        }
    }
}