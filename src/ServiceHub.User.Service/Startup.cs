using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ServiceHub.User.Context.Repositories;

namespace ServiceHub.User.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            const string connectionString = @"mongodb://db";

            services.AddMvc();

            services.AddSingleton<IUserRepository, UserRepository>(serviceProvider =>
            {
                return new UserRepository(
                    new MongoClient(connectionString)
                    .GetDatabase("userdb")
                    .GetCollection<Context.Models.User>("users"));
            });

            services.AddSingleton<UserRepository>(serviceProvider =>
            {
                return new UserRepository(
                    new MongoClient(connectionString)
                    .GetDatabase("userdb")
                    .GetCollection<Context.Models.User>("users"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddApplicationInsights(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
