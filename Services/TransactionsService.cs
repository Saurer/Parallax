using System.Threading.Tasks;
using AuroraCore;
using static Parallax.Graph;

namespace Parallax.Services {
    public class TransactionsService {
        public CredentialsService Credentials { get; private set; }
        public EngineBase Engine { get; private set; }

        public TransactionsService(CredentialsService credentials, EngineBase engine) {
            Credentials = credentials;
            Engine = engine;
        }

        public async Task<int> AssignContainerProperty(int containerID, int propertyID, string value) =>
            await ProcessEvent(containerID, propertyID, containerID, value);

        public async Task<int> AssignProviderAttribute(int providerID, int attributeID) =>
            await ProcessEvent(providerID, StaticEvent.Attribute, providerID, attributeID.ToString());

        public async Task<int> AssignProviderRelation(int providerID, int relationID) =>
            await ProcessEvent(providerID, StaticEvent.Relation, providerID, relationID.ToString());

        public async Task<int> AssignPropertyValueRequirement(int assignationID, int propertyID, bool required) =>
            await ProcessEvent(assignationID, StaticEvent.Required, propertyID, required ? "1" : "0");

        public async Task<int> AssignPropertyValueCardinality(int assignationID, int propertyID, int cardinality) =>
            await ProcessEvent(assignationID, StaticEvent.Cardinality, propertyID, cardinality.ToString());

        public async Task<int> AssignPropertyValuePermission(int assignationID, int propertyID, int actorID) =>
            await ProcessEvent(assignationID, StaticEvent.Permission, propertyID, actorID.ToString());

        public async Task<int> CreateModel(int eventBase, int parentModel, string label) =>
            await ProcessEvent(eventBase, StaticEvent.Model, parentModel, label);

        public async Task<int> CreateIndividual(int eventBase, int modelID, string label) =>
            await ProcessEvent(eventBase, StaticEvent.Individual, modelID, label);

        public async Task<int> CreateAttribute(string label) =>
            await ProcessEvent(StaticEvent.Attribute, StaticEvent.Individual, StaticEvent.AttributeModel, label);

        public async Task<int> CreateEntity(string label) =>
            await ProcessEvent(StaticEvent.Entity, StaticEvent.SubEvent, StaticEvent.Entity, label);

        public async Task<int> AssignAttributeDataType(int attributeID, int dataType) =>
            await ProcessEvent(attributeID, StaticEvent.DataType, attributeID, dataType.ToString());

        public async Task<int> AssignAttributeValue(int attributeID, string value) =>
            await ProcessEvent(attributeID, StaticEvent.AttributeValue, attributeID, value);

        private async Task<int> ProcessEvent(int baseEventID, int valueID, int conditionEventID, string value) {
            var id = Engine.Position;
            var e = new EventData(
                id,
                baseEventID,
                valueID,
                conditionEventID,
                Credentials.CurrentActor.IndividualID,
                value
            );
            await Engine.ProcessEvent(e);
            return id;
        }
    }
}