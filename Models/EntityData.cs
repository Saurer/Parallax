using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class EntityData {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<ModelData> Models { get; private set; }
        public IEnumerable<IndividualData> Individuals { get; private set; }

        private EntityData() {

        }

        public static async Task<EntityData> Instantiate(IEntity entity) {
            var plainModels = await entity.GetModels();
            var plainIndividuals = await entity.GetIndividuals();
            var models = await Task.WhenAll(plainModels.Select(model => ModelData.Instantiate(model)));
            var individuals = await Task.WhenAll(plainIndividuals.Select(individual => IndividualData.Instantiate(individual)));

            return new EntityData {
                ID = entity.ID,
                Name = entity.Value,
                Models = models,
                Individuals = individuals
            };
        }
    }
}