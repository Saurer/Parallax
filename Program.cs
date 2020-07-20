using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AuroraCore;
using AuroraCore.Types;
using AuroraCore.Storage.Implementation;

namespace Parallax {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var engine = await InitEngine();

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton(sp => engine);

            await builder.Build().RunAsync();
        }

        public static async Task<EngineBase> InitEngine() {
            var typeManager = new TypeManager();
            var engine = new EngineBase(new MemoryStorage(typeManager));

            foreach (var e in Graph.Table) {
                await engine.ProcessEvent(e);
            }

            return engine;
        }
    }
}
