using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Service;
using Repositories;
using DomainModels.EF;
using MongoDB.Driver;
using Persistence;
using Microsoft.AspNetCore.Http;

namespace MovieApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddResponseCaching();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMemoryCache();

            //Services
            services.AddScoped<IMovieService, MovieService>();

            //Cache
            services.AddScoped<ICache, MovieCache>();

            //Repositories
            services.AddScoped<IMovieRepository, MovieRepositoryEF>();


            services.AddEntityFrameworkSqlServer();

            //Figure out if development or production environment and register connection information accordingly

            var sqlConnectionString = "";
            var mongoDBConnectionString = "";

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                sqlConnectionString = Configuration.GetConnectionString("prodSQLDB");
                mongoDBConnectionString = Configuration.GetConnectionString("prodMongoDB");
            }
            else {
                sqlConnectionString = Configuration.GetConnectionString("devSQLDB");
                mongoDBConnectionString = Configuration.GetConnectionString("devMongoDB");
            }

            services.AddDbContext<MovieContext>(options => {
                options.UseSqlServer(sqlConnectionString);
            });
            services.AddScoped<IMongoDatabase>(client => new MongoClient(
                MongoClientSettings.FromConnectionString(mongoDBConnectionString))
                .GetDatabase("MoviesDB"));

			services.BuildServiceProvider().GetService<MovieContext>().Database.Migrate();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseResponseCaching();
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
	}
}
