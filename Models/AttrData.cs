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