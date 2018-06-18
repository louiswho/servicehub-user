using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceHub.User.Service
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                DbSeeder.InitializeAsync(services).Wait();
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
            .UseApplicationInsights(Environment.GetEnvironmentVariable("INSTRUMENTATION_KEY"))
            .UseStartup<Startup>()
            .Build();
    }
}
