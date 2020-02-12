using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Mathy.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.Services.AddOptions();
            builder.Services.AddScoped<MathyAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<MathyAuthenticationStateProvider>());
            builder.Services.AddAuthorizationCore();
            //builder.Services.AddScoped<LocalStorage>();
            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();
        }

        public static bool IsLogin = false;
    }
}
