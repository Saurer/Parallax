// Parallax
// Copyright (C) 2020  Frank Horrigan <https://github.com/saurer>

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

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
        public ConditionRule Conditions { get; private set; }

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