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