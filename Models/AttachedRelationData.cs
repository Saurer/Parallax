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
    public class AttachedRelationData {
        public int? AttachmentID { get; private set; }
        public IRelation Relation { get; private set; }
        public bool Required { get; private set; }
        public bool Mutable { get; private set; }
        public int Cardinality { get; private set; }
        public int? Permission { get; private set; }
        public ConditionRule Conditions { get; private set; }
        public PropertyProviderData PropertyProvider { get; private set; }

        public AttachedRelationData(
            IRelation relation,
            bool required,
            int cardinality,
            bool mutable,
            int? permission,
            ConditionRule conditions
        ) : this(relation, required, cardinality, mutable, permission, conditions, null) {
            PropertyProvider = new PropertyProviderData();
        }

        public AttachedRelationData(
            IRelation relation,
            bool required,
            int cardinality,
            bool mutable,
            int? permission,
            PropertyProviderData provider,
            ConditionRule conditions,
            int attachmentID
        ) : this(relation, required, cardinality, mutable, permission, conditions, provider) {
            AttachmentID = attachmentID;
        }

        private AttachedRelationData(
            IRelation relation,
            bool required,
            int cardinality,
            bool mutable,
            int? permission,
            ConditionRule conditions,
            PropertyProviderData provider
        ) {
            Relation = relation;
            Required = required;
            Mutable = mutable;
            Cardinality = cardinality;
            Permission = permission;
            Conditions = conditions;
            PropertyProvider = provider;
        }
    }
}