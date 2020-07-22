using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AuroraCore;
using Parallax.Services;

namespace Parallax.Models {
    public enum IndividualCreateType {
        Actor = StaticEvent.Actor,
        Entity = StaticEvent.Entity,
        Role = StaticEvent.Role
    }

    public class IndividualCreateData {
        [Required, MinLength(1)]
        public string Name { get; set; }

        public Dictionary<int, string> Attributes { get; private set; } = new Dictionary<int, string>();

        public IndividualCreateType Type { get; private set; }

        public int ModelID {
            get {
                return Type switch
                {
                    IndividualCreateType.Actor => StaticEvent.ActorModel,
                    IndividualCreateType.Entity => StaticEvent.EntityModel,
                    IndividualCreateType.Role => StaticEvent.RoleModel,
                    _ => throw new System.Exception("Invalid type")
                };
            }
        }

        public IndividualCreateData(IndividualCreateType type) {
            Type = type;
        }

        public async Task<int> Execute(EngineBase engine, CredentialsService service) {
            int eventID = await service.ProcessEvent(engine, new FederatedEvent(
                (int)Type,
                StaticEvent.Individual,
                ModelID,
                Name
            ));

            foreach (var attr in Attributes) {
                await service.ProcessEvent(engine, new FederatedEvent(eventID, attr.Key, eventID, attr.Value));
            }

            return eventID;
        }
    }
}