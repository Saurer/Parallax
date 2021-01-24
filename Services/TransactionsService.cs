// Parallax
// Copyright (C) 2020  Frank Horrigan <https://github.com/saurer>

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Threading.Tasks;
using AuroraCore;
using AuroraCore.Storage;
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

        public async Task<int> AssignProviderAttribute(int providerID, int attributeID, ConditionRule conditions) =>
            await ProcessEvent(providerID, StaticEvent.Attribute, conditions, attributeID.ToString());

        public async Task<int> AssignProviderRelation(int providerID, int relationID, ConditionRule conditions) =>
            await ProcessEvent(providerID, StaticEvent.Relation, conditions, relationID.ToString());

        public async Task<int> AssignProviderEvent(int providerID, int propertyID, string value, ConditionRule conditions) =>
          await ProcessEvent(providerID, propertyID, conditions, value);

        public async Task<int> AssignPropertyValueRequirement(int assignationID, int propertyID, bool required) =>
            await ProcessEvent(assignationID, StaticEvent.Required, propertyID, required ? "1" : "0");

        public async Task<int> AssignPropertyValueCardinality(int assignationID, int propertyID, int cardinality) =>
            await ProcessEvent(assignationID, StaticEvent.Cardinality, propertyID, cardinality.ToString());

        public async Task<int> AssignPropertyValueMutability(int assignationID, int propertyID, bool mutability) =>
            await ProcessEvent(assignationID, StaticEvent.Mutable, propertyID, mutability ? "1" : "0");

        public async Task<int> AssignPropertyValuePermission(int assignationID, int propertyID, int actorID) =>
            await ProcessEvent(assignationID, StaticEvent.Permission, propertyID, actorID.ToString());

        public async Task<int> AssignPropertyValueSet(int assignationID, int propertyID, string value) =>
            await ProcessEvent(assignationID, StaticEvent.Set, propertyID, value);

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

        private async Task<int> ProcessEvent(int baseEventID, int valueID, int conditionEventID, string value) =>
            await ProcessEvent(baseEventID, valueID, new ConditionRule.EventConditionRule(conditionEventID), value);

        private async Task<int> ProcessEvent(int baseEventID, int valueID, ConditionRule conditions, string value) {
            var id = Engine.Position;
            var e = new EventData(
                id,
                baseEventID,
                valueID,
                conditions,
                Credentials.CurrentActor.IndividualID,
                value
            );
            await Engine.ProcessEvent(e);
            return id;
        }
    }
}