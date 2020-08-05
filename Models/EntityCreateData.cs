using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AuroraCore;
using Parallax.Services;

namespace Parallax.Models {
    public class EntityCreateData {
        [Required, MinLength(1)]
        public string Name { get; set; }
    }
}