using System.Threading.Tasks;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class IndividualData {
        public IIndividual Event { get; private set; }
        public int ID { get; private set; }
        public string Label { get; private set; }
        public int? Actor { get; private set; }
        public string ActorLabel { get; private set; }
        public bool Valid { get; private set; }
        public PropertyProviderData PropertyProvider { get; private set; }
        public IModel Model { get; private set; }

        private IndividualData() {

        }

        public static async Task<IndividualData> Instantiate(IIndividual individual) {
            var model = await individual.GetModel();
            var valid = await individual.Properties.Validate();
            var actorEvent = await individual.GetCreator();
            var provider = await PropertyProviderData.Instantiate(model.ModelID, model.Properties);

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
    }
}