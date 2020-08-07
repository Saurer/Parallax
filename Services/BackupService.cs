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
        public int ConditionEventID { get; set; }
        public int ActorEventID { get; set; }
        public string Value { get; set; }
        public DateTime Date { get; set; }

        public BackupEvent() { }

        public BackupEvent(IEventData e) {
            ID = e.ID;
            BaseEventID = e.BaseEventID;
            ValueID = e.ValueID;
            ConditionEventID = e.ConditionEventID;
            ActorEventID = e.ActorEventID;
            Value = e.Value;
            Date = e.Date;
        }
    }

    public class BackupService {
        private XmlSerializer serializer = new XmlSerializer(typeof(BackupData));

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