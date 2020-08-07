namespace Parallax.Models {
    public class AttrConstraintData {
        public int EventID { get; private set; }
        public int PropertyID { get; private set; }
        public int ValueID { get; private set; }

        public AttrConstraintData(int eventID, int propertyID, int valueID) {
            EventID = eventID;
            PropertyID = propertyID;
            ValueID = valueID;
        }
    }
}