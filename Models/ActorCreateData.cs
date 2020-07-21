using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AuroraCore;
using Parallax.Services;

namespace Parallax.Models {
    public class ActorCreateData {

        [Required, MinLength(1)]
        public string Name { get; set; }

        public Dictionary<int, string> Attributes { get; private set; } = new Dictionary<int, string>();

        public async Task<int> Execute(EngineBase engine, CredentialsService service) {
            int actorID = await service.ProcessEvent(engine, new FederatedEvent(
                StaticEvent.Actor,
                StaticEvent.Individual,
                StaticEvent.ActorModel,
                Name
            ));

            foreach (var attr in Attributes) {
                await service.ProcessEvent(engine, new FederatedEvent(actorID, attr.Key, actorID, attr.Value));
            }

            return actorID;
        }
    }
}