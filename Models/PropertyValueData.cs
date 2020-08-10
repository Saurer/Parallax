using System.Collections.Generic;

namespace Parallax.Models {
    public class PropertyValueData {
        private List<PropertyValueData> containedValueData = new List<PropertyValueData>();

        public int ContainerID { get; private set; }
        public string Value { get; private set; }
        public IEnumerable<PropertyValueData> ContainedValueData => containedValueData;

        public PropertyValueData(int containerID, string value) {
            ContainerID = containerID;
            Value = value;
        }
    }
}