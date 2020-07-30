using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AuroraCore;
using Parallax.Services;

namespace Parallax.Models {
    public class IndividualCreateData {
        [Required, MinLength(1)]
        public string Name { get; set; }

        public Dictionary<int, string> Attributes { get; private set; } = new Dictionary<int, string>();

        public int BaseEvent { get; private set; }

        public int ModelID { get; set; }

        public IndividualCreateData(int baseEvent) {
            BaseEvent = baseEvent;
        }

        public async Task<int> Execute(EngineBase engine, CredentialsService service) {
            int eventID = await service.ProcessEvent(engine, new FederatedEvent(
                BaseEvent,
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