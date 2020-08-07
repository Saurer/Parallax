using AuroraCore.Storage;

namespace Parallax.Models {
    public class BoxedValueData : IBoxedValue {
        public string PlainValue { get; private set; }
        public string ShownValue { get; private set; }

        public BoxedValueData(string plainValue, string shownValue) {
            PlainValue = plainValue;
            ShownValue = shownValue;
        }
    }
}