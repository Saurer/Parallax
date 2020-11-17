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
    public class AttachedAttrData {
        public int? AttachmentID { get; private set; }
        public AttrData Attribute { get; private set; }
        public bool Required { get; private set; }
        public bool Mutable { get; private set; }
        public int Cardinality { get; private set; }
        public int? Permission { get; private set; }
        public IBoxedValue DefaultValue { get; private set; }
        public ConditionRule Conditions { get; private set; }
        public PropertyProviderData PropertyProvider { get; private set; }

        public AttachedAttrData(
            AttrData attr,
            bool required,
            int cardinality,
            bool mutable,
            int? permission,
            IBoxedValue defaultValue,
            ConditionRule conditions
        ) : this(attr, required, cardinality, mutable, permission, defaultValue, conditions, null) {
            PropertyProvider = new PropertyProviderData();
            AttachmentID = null;
        }

        public AttachedAttrData(
            AttrData attr,
            bool required,
            int cardinality,
            bool mutable,
            int? permission,
            IBoxedValue defaultValue,
            PropertyProviderData provider,
            ConditionRule conditions,
            int attachmentID
        ) : this(attr, required, cardinality, mutable, permission, defaultValue, conditions, provider) {
            AttachmentID = attachmentID;
        }

        private AttachedAttrData(
            AttrData attr,
            bool required,
            int cardinality,
            bool mutable,
            int? permission,
            IBoxedValue defaultValue,
            ConditionRule conditions,
            PropertyProviderData provider
        ) {
            Attribute = attr;
            Required = required;
            Cardinality = cardinality;
            Mutable = mutable;
            Permission = permission;
            Conditions = conditions;
            DefaultValue = defaultValue;
            PropertyProvider = provider;
        }
    }
}