using System;
using System.ComponentModel.DataAnnotations;

namespace Parallax.Models {
    public class AddModelAttributeData {
        private bool cardinalityInfinite = false;

        [Required]
        public AttrData Attribute { get; set; }
        public bool Required { get; set; }

        public bool CardinalityInfinite {
            get {
                return cardinalityInfinite;
            }
            set {
                if (value) {
                    Cardinality = 1;
                }

                cardinalityInfinite = value;
            }
        }

        [Range(1, Int32.MaxValue)]
        public int Cardinality { get; set; } = 1;
    }
}