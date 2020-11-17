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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuroraCore.Storage;
using Parallax.Models;

namespace Parallax.Services {
    public class EntityService {
        private readonly TransactionsService tx;
        private readonly IStorageAPI storage;
        private readonly PropertyProviderService propertyProvider;
        private readonly PropertyContainerService propertyContainer;

        public EntityService(
            TransactionsService tx,
            IStorageAPI storage,
            PropertyProviderService propertyProvider,
            PropertyContainerService propertyContainer
        ) {
            this.tx = tx;
            this.storage = storage;
            this.propertyProvider = propertyProvider;
            this.propertyContainer = propertyContainer;
        }

        public async Task<EntityData> GetEntity(int id) {
            var entity = await storage.GetEntity(id);
            return await GetEntity(entity);
        }

        public async Task<EntityData> GetEntity(IEntity entity) {
            var plainModels = await entity.GetModels();
            var plainIndividuals = await entity.GetIndividuals();
            var models = await Task.WhenAll(
                plainModels.Select(model =>
                    propertyProvider.GetModel(model)
                )
            );
            var individuals = await Task.WhenAll(
                plainIndividuals.Select(individual =>
                    propertyContainer.GetIndividual(individual)
                )
            );

            return new EntityData(entity.EntityID, entity.Label, models, individuals);
        }

        public async Task<IEnumerable<EntityData>> GetEntities() {
            var entities = await storage.GetEntities();
            return await Task.WhenAll(
                entities.Select(entity =>
                    GetEntity(entity)
                )
            );
        }
    }
}