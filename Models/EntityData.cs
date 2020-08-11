using System.Collections.Generic;

namespace Parallax.Models {
    public class EntityData {
        public int ID { get; private set; }
        public string Label { get; private set; }
        public IEnumerable<ModelData> Models { get; private set; }
        public IEnumerable<IndividualData> Individuals { get; private set; }

        public EntityData(
            int id,
            string label,
            IEnumerable<ModelData> models,
            IEnumerable<IndividualData> individuals
        ) {
            ID = id;
            Label = label;
            Models = models;
            Individuals = individuals;
        }
    }
}