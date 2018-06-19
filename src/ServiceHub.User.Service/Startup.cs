using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ServiceHub.User.Context.Repositories;
using Swashbuckle.AspNetCore.Swagger;

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

            services.AddSingleton<IUserRepository, UserRepository>();

            services.AddSingleton<IMongoCollection<Context.Models.User>>(mongoCollection =>
            {
                return new MongoClient(connectionString)
                .GetDatabase("userdb")
                .GetCollection<Context.Models.User>("users");
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Revature Housing ServiceHub User API", Version = "v1" });
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

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Revature Housing ServiceHub User API");
            });

            app.UseMvc();
        }
    }
}
