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

using System.Linq;
using System.Collections.Generic;
using AuroraCore.Storage;
using System.Threading.Tasks;
using AuroraCore.Types;

namespace Parallax.Models {
    public class AttrData {
        public int ID { get; private set; }
        public string Label { get; private set; }
        public DataType DataType { get; private set; }
        public bool IsBoxed { get; private set; }
        public IEnumerable<AttrValueData> Values { get; private set; }

        private AttrData() {

        }

        public static async Task<AttrData> Instantiate(IAttr attr) {
            var plainConstraints = await attr.GetConstraints();
            var dataType = await attr.GetDataType();
            var plainValues = await attr.GetValueCandidates();
            var values =
                from v in plainValues
                select new AttrValueData(v.EventValue.ID, v.EventValue.Value);

            return new AttrData {
                ID = attr.PropertyID,
                Label = attr.Label,
                DataType = dataType,
                IsBoxed = dataType.IsBoxed,
                Values = values
            };
        }
    }
}