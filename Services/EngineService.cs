using System.Threading.Tasks;
using AuroraCore;
using AuroraCore.Storage.Implementation;
using AuroraCore.Types;

namespace Parallax.Services {
    public class EngineService {
        public EngineBase Instance { get; private set; }

        public async Task<EngineBase> InitEngine() {
            var typeManager = new TypeManager();
            var engine = new EngineBase(new MemoryStorage(typeManager));

            foreach (var e in Graph.GenesisData) {
                await engine.ProcessEvent(e);
            }

            Instance = engine;
            return Instance;
        }
    }
}