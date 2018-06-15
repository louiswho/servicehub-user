using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ServiceHub.User.Service
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseApplicationInsights(Environment.GetEnvironmentVariable("INSTRUMENTATION_KEY"))
        .UseStartup<Startup>()
        .Build();
  }
}
