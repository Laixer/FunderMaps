using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FunderMaps.Data;
using FunderMaps.Identity;
using FunderMaps.Models.Identity;

namespace FunderMaps
{
    public class Startup
    {
        public readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDatastore(services);
            ConfigureIdentity(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            // In production, the frontend framework files will be served from this directory.
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        private void ConfigureDatastore(IServiceCollection services)
        {
            // Application database
            services.AddDbContext<FunderMapsDbContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString("FunderMapsConnection"), pgOptions =>
                {
                    pgOptions.MigrationsHistoryTable("migrations_history", "meta");
                });
            })
            .AddEntityFrameworkNpgsql();

            // FIS database
            services.AddDbContext<FisDbContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString("FISConnection"));
            })
            .AddEntityFrameworkNpgsql();
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<FunderMapsUser, FunderMapsRole>(options =>
            {
                // Password settings.
                options.Password = Constants.PasswordPolicy;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings.
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<FunderMapsDbContext>()
            .AddDefaultTokenProviders()
            .AddUserManager<FunderMapsUserManager>();

            // Configure identity cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "SpaIdentityToken";
                options.Cookie.IsEssential = true;
                options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
            });
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
                app.UseExceptionHandler("/oops");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");

                //routes.MapSpaFallbackRoute(
                //    name: "spa-fallback",
                //    defaults: new { controller = "home", action = "index" });
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "dev");
                }
            });
        }
    }
}
