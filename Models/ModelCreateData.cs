using System.ComponentModel.DataAnnotations;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class ModelCreateData {

        [Required, MinLength(1)]
        public string Label { get; set; }

        [Required]
        public IEvent EventBase { get; set; }

        [Required]
        public int ParentModelID { get; set; }

        public PropertyProviderData Properties { get; set; }
    }
}