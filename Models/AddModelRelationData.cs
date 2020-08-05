using System;
using System.ComponentModel.DataAnnotations;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class AddModelRelationData {
        [Required]
        public IRelation Relation { get; set; }
        public bool Required { get; set; }

        [Range(0, Int32.MaxValue)]
        public int Cardinality { get; set; } = 1;

        public bool Valid =>
            Cardinality >= 0;
    }
}