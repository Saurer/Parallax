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

            foreach (var attrProto in provider.Attributes) {
                if (null != attrProto.Value.DefaultValue) {
                    AddAttributeValue(attrProto.Key, attrProto.Value.DefaultValue, true);
                }
            }
        }

        public PropertyContainerData(
            Dictionary<int, IEnumerable<PropertyContainerData>> attributes,
            Dictionary<int, IEnumerable<PropertyContainerData>> relations,
            int containerID,
            string actorName,
            IBoxedValue value = null
        ) {
            Value = value;
            ActorName = actorName;
            this.attributes = attributes;
            this.relations = relations;
            this.ContainerID = containerID;
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
            list.Add(new PropertyContainerData(subProvider, value));
        }
        
        public void SetValue(IBoxedValue value) {
            Value = value;
        }

        public void Clear() {
            attributes.Clear();
            relations.Clear();
        }
    }
}