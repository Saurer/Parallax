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
