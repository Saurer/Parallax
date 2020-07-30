using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AuroraCore;
using Parallax.Services;

namespace Parallax.Models {
    public class EntityCreateData {
        [Required, MinLength(1)]
        public string Name { get; set; }

        public async Task<int> Execute(EngineBase engine, CredentialsService service) {
            int eventID = await service.ProcessEvent(engine, new FederatedEvent(
                StaticEvent.Entity,
                StaticEvent.SubEvent,
                StaticEvent.Entity,
                Name
            ));

            return eventID;
        }
    }
}