using System;
using System.ComponentModel.DataAnnotations;
using AuroraCore;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class AddModelRelationData {
        private int cardinality = 1;
        private bool mutable = Const.DefaultMutability;

        [Required]
        public IRelation Relation { get; set; }
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
    }
}