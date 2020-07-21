using System;
using System.Collections.Generic;
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

        public async Task Attach(EngineBase engine, IIndividual actor) {
            await engine.ProcessEvent(new Graph.EventData {
                ID = engine.Position,
                BaseEventID = baseEventID,
                ValueID = valueID,
                ConditionEventID = conditionEventID,
                Value = value,
                ActorEventID = actor.ID,
                Date = DateTime.UtcNow
            });
        }
    }

    public class CredentialsService {

        public IIndividual CurrentActor { get; private set; }

        public void SetCurrentActor(IIndividual actor) {
            CurrentActor = actor;
        }

        public async Task ProcessEvent(EngineBase engine, FederatedEvent e) {
            await e.Attach(engine, CurrentActor);
        }
    }
}