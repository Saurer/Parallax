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
using System.Linq;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class PropertyContainerData {
        private PropertyProviderData provider;
        private Dictionary<int, IEnumerable<PropertyContainerData>> attributes;
        private Dictionary<int, IEnumerable<PropertyContainerData>> relations;

        public int? ContainerID { get; private set; }
        public string ActorName { get; private set; }
        public IReadOnlyDictionary<int, IEnumerable<PropertyContainerData>> Attributes => attributes;
        public IReadOnlyDictionary<int, IEnumerable<PropertyContainerData>> Relations => relations;
        public IBoxedValue Value { get; private set; }
        public bool Fixed { get; private set; }

        public PropertyContainerData(PropertyProviderData provider, IBoxedValue value = null, bool fixedValue = false) {
            this.provider = provider;
            Value = value;
            Fixed = fixedValue;
            attributes = new Dictionary<int, IEnumerable<PropertyContainerData>>();
            relations = new Dictionary<int, IEnumerable<PropertyContainerData>>();
            Initialize();
        }

        public PropertyContainerData(
            PropertyProviderData provider,
            Dictionary<int, IEnumerable<PropertyContainerData>> attributes,
            Dictionary<int, IEnumerable<PropertyContainerData>> relations,
            int containerID,
            string actorName,
            IBoxedValue value = null
        ) {
            this.provider = provider;
            Value = value;
            ActorName = actorName;
            this.attributes = attributes;
            this.relations = relations;
            this.ContainerID = containerID;
            Initialize();
        }

        public void Initialize() {
            foreach (var attrProto in provider.Attributes) {
                if (null != attrProto.Value.DefaultValue) {
                    AddAttributeValue(attrProto.Key, attrProto.Value.DefaultValue, true);
                }
            }
        }

        public bool HasValueFor(int propertyID) {
            if (attributes.TryGetValue(propertyID, out var attributeContainer) && attributeContainer.Any()) {
                return true;
            }

            if (relations.TryGetValue(propertyID, out var relationContainer) && relationContainer.Any()) {
                return true;
            }

            return false;
        }

        public void RemoveAttributeValue(int attributeID, PropertyContainerData container) {
            if (attributes.ContainsKey(attributeID)) {
                var list = (List<PropertyContainerData>)attributes[attributeID];
                list.Remove(container);
            }
        }

        public void AddAttributeValue(int attributeID, IBoxedValue value, bool fixedValue = false) {
            if (!Attributes.ContainsKey(attributeID)) {
                attributes.Add(attributeID, new List<PropertyContainerData>());
            }

            var list = (List<PropertyContainerData>)attributes[attributeID];
            var subProvider = provider.Attributes[attributeID].PropertyProvider;

            if (provider.Attributes[attributeID].Mutable) {
                list.Clear();
            }

            list.Add(new PropertyContainerData(subProvider, value, fixedValue));
        }

        public void RemoveRelationValue(int relationID, PropertyContainerData container) {
            if (relations.ContainsKey(relationID)) {
                var list = (List<PropertyContainerData>)relations[relationID];
                list.Remove(container);
            }
        }

        public void AddRelationValue(int relationID, IBoxedValue value) {
            if (!Relations.ContainsKey(relationID)) {
                relations.Add(relationID, new List<PropertyContainerData>());
            }

            var list = (List<PropertyContainerData>)relations[relationID];
            var subProvider = provider.Relations[relationID].PropertyProvider;

            if (provider.Relations[relationID].Mutable) {
                list.Clear();
            }

            list.Add(new PropertyContainerData(subProvider, value));
        }

        public void SetValue(IBoxedValue value) {
            Value = value;
        }

        public void Clear() {
            attributes.Clear();
            relations.Clear();
            Initialize();
        }
    }
}