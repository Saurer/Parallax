namespace Parallax.Models {
    public class ModelData {
        public int ID { get; set; }
        public string Label { get; set; }
        public int Parent { get; set; }
        public string ParentLabel { get; set; }
        public int EventBase { get; set; }
        public string EventBaseName { get; set; }
        public PropertyProviderData PropertyProvider { get; set; }
    }
}