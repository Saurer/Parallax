using System;
using System.Threading.Tasks;
using AuroraCore.Storage;

namespace Parallax.Models {
    public class BoxedValueData : IBoxedValue {
        public string PlainValue { get; private set; }
        public string ShownValue { get; private set; }

        public int AssignationID { get; private set; }
        public IEventData EventValue { get; private set; }
        public DateTime Date { get; private set; }

        public BoxedValueData(IEventData e, string shownValue) {
            AssignationID = e.ID;
            EventValue = e;
            Date = e.Date;
            PlainValue = e.Value;
            ShownValue = shownValue;
        }

        public BoxedValueData(string plainValue, string shownValue) {
            PlainValue = plainValue;
            ShownValue = shownValue;
        }

        public Task<IIndividual> GetCreator() {
            throw new NotImplementedException();
        }

        public Task<IEvent> GetConditionEvent() {
            throw new NotImplementedException();
        }

        public Task<IEvent> GetBaseEvent() {
            throw new NotImplementedException();
        }

        public Task<IEvent> GetValueTypeEvent() {
            throw new NotImplementedException();
        }
    }
}