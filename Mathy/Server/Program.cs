using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace Mathy.Server
{
    public class Program
    {
        private static IServiceProvider _ServiceProvider;
        public static void Main(string[] args)
        {
            var webHost = BuildWebHost(args);
            _ServiceProvider = webHost.Services;
            webHost.Run();
        }

        public static T GetService<T>()
        {
            return (T)_ServiceProvider.GetService(typeof(T));
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseConfiguration(new ConfigurationBuilder()
                //    .AddCommandLine(args)
                //    .Build())
                .UseUrls("http://*:8081")
                .UseStartup<Startup>()            
                .Build();
    }
}
