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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuroraCore.Storage;
using Parallax.Models;

namespace Parallax.Services {
    public class PropertyContainerService {
        private readonly DialogService dialog;
        private readonly TransactionsService tx;
        private readonly IStorageAPI storage;
        private readonly PropertyProviderService propertyProvider;

        public PropertyContainerService(DialogService dialog, TransactionsService tx, IStorageAPI storage, PropertyProviderService propertyProvider) {
            this.dialog = dialog;
            this.tx = tx;
            this.storage = storage;
            this.propertyProvider = propertyProvider;
        }

        public async Task<int> CreateIndividual(IndividualCreateData model) {
            int eventID = await tx.CreateIndividual(model.BaseEvent, model.ModelID, model.Label);
            if (null != model.Properties) {
                await CreateScopedProperties(eventID, model.Properties.Attributes, container => container.Attributes);
                await CreateScopedProperties(eventID, model.Properties.Relations, container => container.Relations);
            }
            return eventID;
        }

        public async Task<IndividualData> GetIndividual(int id) {
            var individual = await storage.GetIndividual(id);
            return await GetIndividual(individual);
        }

        public async Task<IndividualData> GetIndividual(IIndividual individual) {
            var model = await individual.GetModel();
            var valid = await individual.Properties.Validate();
            var actorEvent = await individual.GetCreator();
            var provider = await propertyProvider.GetProvider(model.ModelID);

            return new IndividualData {
                Event = individual,
                ID = individual.IndividualID,
                Label = individual.Label,
                Actor = actorEvent?.IndividualID,
                ActorLabel = actorEvent?.Label,
                Valid = valid,
                PropertyProvider = provider,
                Model = model
            };
        }

        public async Task<PropertyContainerData> GetPropertyContainer(int containerID, IBoxedValue value = null) {
            var container = await storage.GetPropertyContainer(containerID);
            var plainAttributes = await container.GetAttributes();
            var plainRelations = await container.GetRelations();
            var resultAttributes = new Dictionary<int, IEnumerable<PropertyContainerData>>();
            var resultRelations = new Dictionary<int, IEnumerable<PropertyContainerData>>();
            var containerEvent = await storage.GetEvent(containerID);
            var actorEvent = await storage.GetEvent(containerEvent.EventValue.ActorEventID);
            var parentEvent = await storage.GetEvent(containerEvent.EventValue.BaseEventID);
            var providerEvent = await parentEvent.GetConditionEvent();
            var provider = await propertyProvider.GetProvider(providerEvent.EventValue.ID);

            foreach (var kv in plainAttributes) {
                var newValues = await Task.WhenAll(
                    kv.Value.Select(async v =>
                        await GetPropertyContainer(v.AssignationID, v)
                    )
                );
                resultAttributes.Add(kv.Key, newValues);
            }
            foreach (var kv in plainRelations) {
                var newValues = await Task.WhenAll(
                    kv.Value.Select(async v =>
                        await GetPropertyContainer(v.AssignationID, v)
                    )
                );
                resultRelations.Add(kv.Key, newValues);
            }

            return new PropertyContainerData(
                provider,
                resultAttributes,
                resultRelations,
                containerID,
                actorEvent.EventValue.Value,
                value
            );
        }

        private async Task CreateScopedProperties(
            int containerID,
            IReadOnlyDictionary<int, IEnumerable<PropertyContainerData>> properties,
            Func<PropertyContainerData, IReadOnlyDictionary<int, IEnumerable<PropertyContainerData>>> scopeSelector
        ) {
            foreach (var property in properties) {
                foreach (var propertyValue in property.Value) {
                    var assignationID = await tx.AssignContainerProperty(containerID, property.Key, propertyValue.Value.PlainValue);
                    await CreateScopedProperties(assignationID, scopeSelector(propertyValue), scopeSelector);
                }
            }
        }
    }
}