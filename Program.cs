using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazored.Modal;
using Parallax.Services;
using System.Linq;
using AuroraCore.Storage;

namespace Parallax {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var engineService = await EngineService.Instantiate();
            var credentialsService = new CredentialsService();
            var actors = await engineService.Instance.Storage.GetActors();
            credentialsService.SetCurrentActor(actors.First());

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton(sp => engineService.Instance);
            builder.Services.AddSingleton(sp => engineService);
            builder.Services.AddSingleton(sp => credentialsService);
            builder.Services.AddSingleton(sp => new BackupService());
            builder.Services.AddSingleton(sp => new RoutingService());
            builder.Services.AddSingleton(typeof(IStorageAPI), engineService.Instance.Storage);

            builder.Services.AddScoped(typeof(DialogService));
            builder.Services.AddScoped(typeof(TransactionsService));
            builder.Services.AddScoped(typeof(PropertyProviderService));
            builder.Services.AddScoped(typeof(PropertyContainerService));
            builder.Services.AddScoped(typeof(EntityService));
            builder.Services.AddScoped(typeof(ConditionsService));
            builder.Services.AddBlazoredModal();

            await builder.Build().RunAsync();
        }
    }
}
