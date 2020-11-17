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