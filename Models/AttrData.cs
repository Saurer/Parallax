using System.Linq;
using System.Collections.Generic;
using AuroraCore.Storage;
using System.Threading.Tasks;
using System;
using AuroraCore.Types;

namespace Parallax.Models {
    public class AttrData {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public DataType DataType { get; private set; }
        public bool IsBoxed { get; private set; }
        public IEnumerable<AttrPropertyData> Properties { get; private set; }
        public IEnumerable<AttrValueData> Values { get; private set; }

        private AttrData() {

        }

        public static async Task<AttrData> Instantiate(IAttr attr) {
            var plainProperties = await attr.GetProperties();
            var plainValues = await attr.GetValues();
            var dataType = await attr.GetDataType();
            var properties =
                from p in plainProperties
                select new AttrPropertyData(p.BaseEventID, p.ValueID, Int32.Parse(p.Value));
            var values =
                from v in plainValues
                select new AttrValueData(v.ID, v.Value);

            return new AttrData {
                ID = attr.ID,
                Name = attr.Value,
                DataType = dataType,
                IsBoxed = dataType.IsBoxed,
                Properties = properties,
                Values = values
            };
        }
    }
}