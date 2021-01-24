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

namespace Parallax.Models {
    public class PropertyProviderData {
        private Dictionary<int, AttachedAttrData> attributes;
        private Dictionary<int, AttachedRelationData> relations;
        private Dictionary<int, AttachedEventData> events;

        public int? ProviderID { get; private set; }
        public IReadOnlyDictionary<int, AttachedAttrData> Attributes => attributes;
        public IReadOnlyDictionary<int, AttachedRelationData> Relations => relations;
        public IReadOnlyDictionary<int, AttachedEventData> Events => events;

        public PropertyProviderData() {
            attributes = new Dictionary<int, AttachedAttrData>();
            relations = new Dictionary<int, AttachedRelationData>();
            events = new Dictionary<int, AttachedEventData>();
        }

        public PropertyProviderData(
            IReadOnlyDictionary<int, AttachedAttrData> attributes,
            IReadOnlyDictionary<int, AttachedRelationData> relations,
            IReadOnlyDictionary<int, AttachedEventData> events,
            int providerID
        ) {
            this.attributes = new Dictionary<int, AttachedAttrData>(attributes);
            this.relations = new Dictionary<int, AttachedRelationData>(relations);
            this.events = new Dictionary<int, AttachedEventData>(events);
            this.ProviderID = providerID;
        }

        public void AddAttribute(AttachedAttrData attr) {
            attributes.Add(attr.Attribute.ID, attr);
        }

        public void AddRelation(AttachedRelationData relation) {
            relations.Add(relation.Relation.PropertyID, relation);
        }

        public void AddEvent(AttachedEventData eventData) {
            events.Add(eventData.Property.PropertyID, eventData);
        }

        public void RemoveAttribute(AttachedAttrData attr) {
            attributes.Remove(attr.Attribute.ID);
        }

        public void RemoveRelation(AttachedRelationData relation) {
            relations.Remove(relation.Relation.PropertyID);
        }

        public void RemoveEvent(AttachedEventData eventData) {
            events.Remove(eventData.Property.PropertyID);
        }
    }
}