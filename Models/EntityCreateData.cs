using System.ComponentModel.DataAnnotations;

namespace Parallax.Models {
    public class EntityCreateData {
        [Required, MinLength(1)]
        public string Label { get; set; }
    }
}