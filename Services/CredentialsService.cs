using System;
using System.Threading.Tasks;
using AuroraCore;
using AuroraCore.Storage;

namespace Parallax.Services {
    public class FederatedEvent {
        private int baseEventID;
        private int valueID;
        private int conditionEventID;
        private string value;

        public FederatedEvent(int baseEventID, int valueID, int conditionEventID, string value) {
            this.baseEventID = baseEventID;
            this.valueID = valueID;
            this.conditionEventID = conditionEventID;
            this.value = value;
        }

        public async Task<int> Attach(EngineBase engine, IIndividual actor) {
            int id = engine.Position;
            await engine.ProcessEvent(new Graph.EventData {
                ID = id,
                BaseEventID = baseEventID,
                ValueID = valueID,
                ConditionEventID = conditionEventID,
                Value = value,
                ActorEventID = actor.ID,
                Date = DateTime.UtcNow
            });
            return id;
        }
    }

    public class CredentialsService {

        public IIndividual CurrentActor { get; private set; }

        public void SetCurrentActor(IIndividual actor) {
            CurrentActor = actor;
        }

        public async Task<int> ProcessEvent(EngineBase engine, FederatedEvent e) {
            return await e.Attach(engine, CurrentActor);
        }
    }
}