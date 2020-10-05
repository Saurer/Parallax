using System;
using System.ComponentModel.DataAnnotations;

namespace Parallax.Models {
    public class AddModelAttributeData {
        [Required]
        public AttrData Attribute { get; set; }
        public bool Required { get; set; }

        [Range(0, Int32.MaxValue)]
        public int Cardinality { get; set; } = 1;

        public int? Permission { get; set; }

        public bool Valid =>
            Cardinality >= 0;

        public ConditionData Conditions { get; set; } = new ConditionData();

        public AttachedAttrData AttributeModel {
            get {
                return new AttachedAttrData(Attribute, Required, Cardinality, Permission, null, null);
            }
        }
    }
}