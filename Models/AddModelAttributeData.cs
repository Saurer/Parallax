using System;
using System.ComponentModel.DataAnnotations;

namespace Parallax.Models {
    public class AddModelAttributeData {
        [Required]
        public AttrData Attribute { get; set; }
        public bool Required { get; set; }

        [Range(0, Int32.MaxValue)]
        public int Cardinality { get; set; } = 1;

        public bool Valid =>
            Cardinality >= 0;
    }
}