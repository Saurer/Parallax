using System;
using System.ComponentModel.DataAnnotations;
using AuroraCore;

namespace Parallax.Models {
    public class AddModelAttributeData {
        private int cardinality = 1;
        private bool mutable = Const.DefaultMutability;

        [Required]
        public AttrData Attribute { get; set; }
        public bool Required { get; set; } = Const.DefaultRequired;
        public bool Mutable {
            get {
                return mutable;
            }
            set {
                if (value) {
                    cardinality = 1;
                }

                mutable = value;
            }
        }

        [Range(0, Int32.MaxValue)]
        public int Cardinality {
            get {
                return Mutable ? 1 : cardinality;
            }
            set {
                cardinality = value;
            }
        }

        public int? Permission { get; set; }

        public bool Valid =>
            Cardinality >= 0;

        public ConditionData Conditions { get; set; } = new ConditionData();

        public AttachedAttrData AttributeModel {
            get {
                return new AttachedAttrData(Attribute, Required, Cardinality, Mutable, Permission, null, null);
            }
        }
    }
}