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
    }
}