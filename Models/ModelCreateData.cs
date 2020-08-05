using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Parallax.Models {
    public class ModelCreateData {
        private List<AddModelAttributeData> attributes = new List<AddModelAttributeData>();

        [Required, MinLength(1)]
        public string Name { get; set; }

        public IEnumerable<AddModelAttributeData> Attributes {
            get {
                return attributes;
            }
        }

        public void AddAttribute(AddModelAttributeData value) {
            attributes.Add(value);
        }

        public void RemoveAttribute(int id) {
            var attr = attributes.Where(l => l.Attribute.ID == id).SingleOrDefault();
            attributes.Remove(attr);
        }

        public bool HasAttribute(int id) {
            return attributes.Any(a => a.Attribute.ID == id);
        }
    }
}