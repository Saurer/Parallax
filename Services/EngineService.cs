using System.Collections.Generic;
using System.Threading.Tasks;
using AuroraCore;
using AuroraCore.Storage;
using AuroraCore.Storage.Implementation;
using AuroraCore.Types;

namespace Parallax.Services {
    public class EngineService {
        public EngineBase Instance { get; private set; }
        public TypeManager Types { get; private set; }

        public async Task<EngineBase> InitEngine() {
            return await InitEngine(Graph.GenesisData);
        }

        public async Task<EngineBase> InitEngine(IEnumerable<IEvent> initialGraph) {
            var typeManager = new TypeManager();
            var engine = new EngineBase(new MemoryStorage(typeManager));

            foreach (var e in initialGraph) {
                await engine.ProcessEvent(e);
            }

            Instance = engine;
            Types = typeManager;
            return Instance;
        }
    }
}