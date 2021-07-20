using Daytona.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;
using Vienna.Dal;
using Vienna.Dal.Models;

namespace Vienna
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddUseFriendlyExceptionsMiddleware();

            if (!Environment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(Configuration["Redis:Uri"]);
                services.AddDataProtection().PersistKeysToStackExchangeRedis(redis);
            }

            services.AddDbContext<ViennaDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("ViennaConnection")));

            services.AddIdentityCore<ApplicationUser>(o =>
            {
                o.Stores.MaxLengthForKeys = 128;
                o.SignIn.RequireConfirmedAccount = true;
                o.Password.RequiredLength = 8;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequiredUniqueChars = 1;
                o.Lockout.MaxFailedAccessAttempts = 3;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(2);
            });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "vienna-app/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "vienna-app";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
