using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazored.Modal;
using Parallax.Services;

namespace Parallax {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var engineService = new EngineService();
            await engineService.InitEngine();

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddTransient(sp => engineService.Instance);
            builder.Services.AddSingleton(sp => engineService);
            builder.Services.AddBlazoredModal();

            await builder.Build().RunAsync();
        }
    }
}
