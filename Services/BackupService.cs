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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AuroraCore.Storage;

namespace Parallax.Services {
    [XmlRootAttribute("BackupData", IsNullable = false)]
    public class BackupData {
        public BackupEvent[] Events;
    }

    public class BackupEvent : IEventData {
        public int ID { get; set; }
        public int BaseEventID { get; set; }
        public int ValueID { get; set; }
        public ConditionRule Conditions { get; set; }
        public int ActorEventID { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }

        public BackupEvent() { }

        public BackupEvent(IEventData e) {
            ID = e.ID;
            BaseEventID = e.BaseEventID;
            ValueID = e.ValueID;
            Conditions = e.Conditions;
            ActorEventID = e.ActorEventID;
            Value = e.Value;
            Date = e.Date;
        }
    }

    public class BackupService {
        private XmlSerializer serializer;

        public BackupService() {
            Type[] extraTypes = {
                typeof(ConditionRule),
                typeof(ConditionRule.EventConditionRule),
                typeof(ConditionRule.ConjunctionRule),
                typeof(ConditionRule.DisjunctionRule),
                typeof(ConditionRule.PropertyEqualityRule),
                typeof(ConditionRule.PropertyInequalityRule)
            };
            serializer = new XmlSerializer(typeof(BackupData), extraTypes);
        }

        public BackupData DecodeFromBase64(string data) {
            var bytes = Convert.FromBase64String(data);
            return Decode(bytes);
        }

        public async Task<string> Encode(IEnumerable<IEventData> events) {
            using (var writer = new MemoryStream())
            using (var reader = new StreamReader(writer)) {
                serializer.Serialize(writer, new BackupData {
                    Events = events.Select(e => new BackupEvent(e)).ToArray()
                });
                writer.Position = 0;
                var result = await reader.ReadToEndAsync();
                return result;
            }
        }

        public BackupData Decode(byte[] data) {
            using (var stream = new MemoryStream(data)) {
                var result = (BackupData)serializer.Deserialize(stream);
                return result;
            }
        }
    }
}