using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parallax.Models {
    public class IndividualCreateData {
        [Required, MinLength(1)]
        public string Name { get; set; }

        public Dictionary<int, IEnumerable<string>> Attributes { get; private set; } = new Dictionary<int, IEnumerable<string>>();
        public Dictionary<int, IEnumerable<string>> Relations { get; private set; } = new Dictionary<int, IEnumerable<string>>();

        public int BaseEvent { get; private set; }

        public int ModelID { get; set; }

        public IndividualCreateData(int baseEvent) {
            BaseEvent = baseEvent;
        }
    }
}