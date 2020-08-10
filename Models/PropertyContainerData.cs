using System.Collections.Generic;
using System.Linq;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class PropertyContainerData {
        private Dictionary<int, IEnumerable<PropertyContainerData>> attributes;
        private Dictionary<int, IEnumerable<PropertyContainerData>> relations;

        public int? ContainerID { get; private set; }
        public string ActorName { get; private set; }
        public IReadOnlyDictionary<int, IEnumerable<PropertyContainerData>> Attributes => attributes;
        public IReadOnlyDictionary<int, IEnumerable<PropertyContainerData>> Relations => relations;
        public IBoxedValue Value { get; private set; }

        public PropertyContainerData(IBoxedValue value = null) {
            Value = value;
            attributes = new Dictionary<int, IEnumerable<PropertyContainerData>>();
            relations = new Dictionary<int, IEnumerable<PropertyContainerData>>();
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

        public void AddAttributeValue(int attributeID, IBoxedValue value) {
            if (!Attributes.ContainsKey(attributeID)) {
                attributes.Add(attributeID, new List<PropertyContainerData>());
            }

            var list = (List<PropertyContainerData>)attributes[attributeID];
            list.Add(new PropertyContainerData(value));
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
            list.Add(new PropertyContainerData(value));
        }
        
        public void SetValue(IBoxedValue value) {
            Value = value;
        }
    }
}