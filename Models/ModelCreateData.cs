using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AuroraCore;
using Parallax.Services;

namespace Parallax.Models {
    public enum EventType {
        Actor = StaticEvent.Actor,
        Attribute = StaticEvent.Attribute,
        Entity = StaticEvent.Entity,
    }

    public class ModelCreateData {
        private List<(string, int)> attributes = new List<(string, int)>();

        [Required, MinLength(1)]
        public string Name { get; set; }

        [Required]
        public int Parent { get; set; }

        [Required]
        public EventType EventType { get; set; } = EventType.Attribute;

        public IEnumerable<(string, int)> Attributes {
            get {
                return attributes;
            }
        }

        public void AddAttribute((string, int) attr) {
            attributes.Add(attr);
        }

        public void RemoveAttribute((string, int) attr) {
            attributes.Remove(attr);
        }

        public async Task<int> Execute(EngineBase engine, CredentialsService service) {
            // Create model
            int modelID = await service.ProcessEvent(engine, new FederatedEvent((int)EventType, StaticEvent.Model, Parent, Name));

            // Create attributes
            foreach (var (name, dataType) in attributes) {
                int attrID = await service.ProcessEvent(engine, new FederatedEvent(StaticEvent.Attribute, StaticEvent.Individual, StaticEvent.AttributeModel, name));
                await service.ProcessEvent(engine, new FederatedEvent(attrID, StaticEvent.DataType, attrID, dataType.ToString()));
                await service.ProcessEvent(engine, new FederatedEvent(modelID, StaticEvent.Attribute, modelID, attrID.ToString()));
            }

            return modelID;
        }
    }
}