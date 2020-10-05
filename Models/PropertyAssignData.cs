using AuroraCore.Storage;

namespace Parallax.Models {
    public class PropertyAssignData {
        public int ID { get; set; }
        public bool Required { get; set; }
        public int Cardinality { get; set; }
        public int? Permission { get; set; }
        public ConditionRule Conditions { get; set; }
        public IBoxedValue DefaultValue { get; set; }
    }
}