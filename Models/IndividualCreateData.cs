using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class IndividualCreateData {
        [Required, MinLength(1)]
        public string Name { get; set; }

        public Dictionary<int, IEnumerable<IBoxedValue>> Attributes { get; private set; } = new Dictionary<int, IEnumerable<IBoxedValue>>();
        public Dictionary<int, IEnumerable<IBoxedValue>> Relations { get; private set; } = new Dictionary<int, IEnumerable<IBoxedValue>>();

        public int BaseEvent { get; private set; }

        public int ModelID { get; set; }

        public IndividualCreateData(int baseEvent) {
            BaseEvent = baseEvent;
        }
    }
}