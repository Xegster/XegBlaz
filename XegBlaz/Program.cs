using Fluxor;
using XegBlaz.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XegBlaz
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			
			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			builder.Services.AddFluxor(options => options.ScanAssemblies(typeof(Program).Assembly).UseReduxDevTools());

			builder.Services.AddScoped<IApiService, ApiService>();

			await builder.Build().RunAsync();
		}
	}
}
