namespace Parallax.Models {
    public class AttrValueData {
        public int EventID { get; private set; }
        public string Value { get; private set; }

        public AttrValueData(int eventID, string value) {
            EventID = eventID;
            Value = value;
        }
    }
}