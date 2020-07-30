using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AuroraCore;
using Parallax.Services;

namespace Parallax.Models {
    public class ModelCreateData {
        private List<AttrData> attributes = new List<AttrData>();

        [Required, MinLength(1)]
        public string Name { get; set; }

        [Required]
        public int Parent { get; set; }

        [Required]
        public int EventType { get; set; }

        public IEnumerable<AttrData> Attributes {
            get {
                return attributes;
            }
        }

        public void AddAttribute(AttrData value) {
            attributes.Add(value);
        }

        public void RemoveAttribute(AttrData value) {
            attributes.Remove(value);
        }

        public bool HasAttribute(AttrData value) {
            return attributes.Contains(value);
        }

        public bool HasAttribute(int id) {
            return attributes.Any(a => a.ID == id);
        }

        public async Task<int> Execute(EngineBase engine, CredentialsService service) {
            // Create model
            int modelID = await service.ProcessEvent(engine, new FederatedEvent(EventType, StaticEvent.Model, Parent, Name));

            // Attach attributes
            foreach (var attr in attributes) {
                await service.ProcessEvent(engine, new FederatedEvent(modelID, StaticEvent.Attribute, modelID, attr.ID.ToString()));
            }

            return modelID;
        }
    }
}