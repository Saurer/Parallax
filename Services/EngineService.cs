using System.Threading.Tasks;
using AuroraCore;
using AuroraCore.Storage.Implementation;
using AuroraCore.Types;

namespace Parallax.Services {
    public class EngineService {
        public EngineBase Instance { get; private set; }
        public TypeManager Types { get; private set; }

        private EngineService(EngineBase engine, TypeManager types) {
            Instance = engine;
            Types = types;
        }

        public static async Task<EngineService> Instantiate() {
            var typeManager = new TypeManager();
            var engine = new EngineBase(new MemoryStorage(typeManager));

            foreach (var e in Graph.GenesisData) {
                await engine.ProcessEvent(e);
            }

            return new EngineService(engine, typeManager);
        }
    }
}