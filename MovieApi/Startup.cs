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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Swagger;

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

            // Add authentication (Azure AD) 
            services
             .AddAuthentication(sharedOptions =>
             {
                 sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                 sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             })
             .AddJwtBearer(options =>
             {
                 options.Audience = Configuration["AzureAd:ClientId"];
                 options.Authority = $"{Configuration["AzureAd:Instance"]}{Configuration["AzureAd:TenantId"]}";
                 options.TokenValidationParameters = new TokenValidationParameters { RoleClaimType = ClaimTypes.Role };
             });

            services.AddResponseCaching();
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMemoryCache();

            //Services
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDateService, DateService>();

            //Cache
            services.AddScoped<ICache, MovieCache>();

            //Repositories
            services.AddScoped<IMovieRepository, MovieRepositoryEF>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "Movie API", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme {
                    In = "header",
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey",
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });
            });

			services.BuildServiceProvider().GetService<MovieContext>().Database.Migrate();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
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

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
