using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using dexConvert;
using dexConvert.Repository;
using dexConvert.Services;
using dexConvert.Worker;
using MudBlazor.Services;


WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddHttpClient();
builder.Services.AddMudServices();
builder.Services.AddScoped<IDownloadWorker, DownloadWorker>();
builder.Services.AddScoped<IPreferenceService, PreferenceService>();
builder.Services.AddScoped<IMangaService, MangaService>();
builder.Services.AddScoped<IApiRepository, ApiRepository>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });




await builder.Build().RunAsync();