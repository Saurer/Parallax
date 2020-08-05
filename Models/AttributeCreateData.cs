using System.ComponentModel.DataAnnotations;

namespace Parallax.Models {
    public class AttributeCreateData {

        [Required, MinLength(1)]
        public string Name { get; set; }

        [Required]
        public int DataType { get; set; }
    }
}