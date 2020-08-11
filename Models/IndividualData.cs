using AuroraCore.Storage;

namespace Parallax.Models {
    public class IndividualData {
        public IIndividual Event { get; set; }
        public int ID { get; set; }
        public string Label { get; set; }
        public int? Actor { get; set; }
        public string ActorLabel { get; set; }
        public bool Valid { get; set; }
        public PropertyProviderData PropertyProvider { get; set; }
        public IModel Model { get; set; }
    }
}