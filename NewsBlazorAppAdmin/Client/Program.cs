using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NewsBlazorAppAdmin.Services;

namespace NewsBlazorAppAdmin.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddTransient<AuthorizationMessageHandler>();

            builder.Services.AddHttpClient("Client", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7083/");
            }).AddHttpMessageHandler<AuthorizationMessageHandler>();

            builder.Services.AddScoped<NewsService>();
            builder.Services.AddScoped<ApiServices>();
            builder.Services.AddScoped<SectionsService>();
            builder.Services.AddScoped<Top15NewsService>();
            builder.Services.AddScoped<AdminsServices>();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddBlazoredLocalStorage();
            var run = builder.Build();
            await run.RunAsync();
        }
    }
}