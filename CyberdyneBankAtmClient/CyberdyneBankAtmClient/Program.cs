using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using CyberdyneBankAtmClient;
using CyberdyneBankAtmClient.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSingleton<NotificationService>();


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddScoped<IAccountApiService, AccountApiService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:8081/") });

await builder.Build().RunAsync();