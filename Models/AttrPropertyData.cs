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