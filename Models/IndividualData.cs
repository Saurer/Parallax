using System.Collections.Generic;
using System.Threading.Tasks;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class IndividualData {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public IndividualModelData Model { get; private set; }
        public IReadOnlyDictionary<int, string> Attributes { get; private set; }
        public bool Valid { get; private set; }

        private IndividualData() {

        }

        public static async Task<IndividualData> Instantiate(IIndividual individual) {
            var plainModel = await individual.GetModel();
            var attributes = await individual.GetAttributes();
            var model = await IndividualModelData.Instantiate(plainModel, attributes);
            var values = await individual.GetAttributes();
            var valid = await plainModel.Validate(values);

            return new IndividualData {
                ID = individual.ID,
                Name = individual.Value,
                Attributes = attributes,
                Model = model,
                Valid = valid
            };
        }
    }
}