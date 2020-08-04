using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AuroraCore;
using Parallax.Services;

namespace Parallax.Models {
    public class ModelCreateData {
        private List<AddModelAttributeData> attributes = new List<AddModelAttributeData>();

        [Required, MinLength(1)]
        public string Name { get; set; }

        [Required]
        public int Parent { get; set; }

        [Required]
        public int EventType { get; set; }

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

        public async Task<int> Execute(EngineBase engine, CredentialsService service) {
            // Create model
            int modelID = await service.ProcessEvent(engine, new FederatedEvent(EventType, StaticEvent.Model, Parent, Name));

            // Attach attributes
            foreach (var attr in attributes) {
                var eventID = await service.ProcessEvent(engine, new FederatedEvent(modelID, StaticEvent.Attribute, modelID, attr.Attribute.ID.ToString()));

                if ((Const.DefaultRequired == 1) != attr.Required) {
                    await service.ProcessEvent(engine, new FederatedEvent(
                        eventID,
                        StaticEvent.Required,
                        attr.Attribute.ID,
                        attr.Required ? "1" : "0"
                    ));
                }

                if (Const.DefaultCardinality != attr.Cardinality) {
                    await service.ProcessEvent(engine, new FederatedEvent(
                        eventID,
                        StaticEvent.Cardinality,
                        attr.Attribute.ID,
                        attr.Cardinality.ToString()
                    ));
                }
            }

            return modelID;
        }
    }
}