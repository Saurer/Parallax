using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AuroraCore;
using Parallax.Services;

namespace Parallax.Models {
    public class AttributeCreateData {

        [Required, MinLength(1)]
        public string Name { get; set; }

        [Required]
        public int DataType { get; set; }

        public async Task<int> Execute(EngineBase engine, CredentialsService service) {
            int attrID = await service.ProcessEvent(engine, new FederatedEvent(StaticEvent.Attribute, StaticEvent.Individual, StaticEvent.AttributeModel, Name));
            await service.ProcessEvent(engine, new FederatedEvent(attrID, StaticEvent.DataType, attrID, DataType.ToString()));
            return attrID;
        }
    }
}