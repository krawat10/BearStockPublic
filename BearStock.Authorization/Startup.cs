using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using BearStock.Authorization.Authorization;
using BearStock.Authorization.Helpers;
using BearStock.Authorization.Managers;
using BearStock.Authorization.Models;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BearStock.Authorization
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
            string? migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddCors(options =>
            {
                options.AddPolicy("API",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            services.AddMvc();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DataContext")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<ClientTokenSettings>(Configuration.GetSection("ClientTokenSettings"));

            services.AddTransient<UserTokenProvider>();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder
                        .UseSqlServer(Configuration.GetConnectionString("DataContext"),
                            sql => sql.MigrationsAssembly(migrationAssembly));
                })
                // Operational Store (tokens, consents ...)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(
                            Configuration.GetConnectionString("DataContext"),
                            sql => sql.MigrationsAssembly(migrationAssembly)
                        );
                })
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<IdentityProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("API");

            app.Map("/authentication", builder =>
            {
                builder.UseRouting();

                builder.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        "default",
                        "{controller=Users}/{action=Authenticate}");
                });

                builder.UseIdentityServer();
            });

            InitializeIdentityServerDatabase(app);
        }

        private void InitializeIdentityServerDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            while (true)
                try
                {
                    serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                    serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                    serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
                    break;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Waiting for db.");
                    Thread.Sleep(5000);
                }

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var user = userManager.FindByNameAsync("username").Result;

            if (user == null)
            {
                user = new ApplicationUser {
                    UserName = "username", Email = "username@username.com", EmailConfirmed = true
                };

                userManager.CreateAsync(user, "Password123$").GetAwaiter().GetResult();

                userManager.AddClaimsAsync(user,
                    new[] {new Claim(JwtClaimTypes.Name, user.UserName)}).GetAwaiter().GetResult();
            }

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            context.Clients.RemoveRange(context.Clients);
            context.ApiScopes.RemoveRange(context.ApiScopes);
            context.ApiResources.RemoveRange(context.ApiResources);
            context.SaveChanges();

            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients) context.Clients.Add(client.ToEntity());

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var apiResource in Config.ApiResources) context.ApiResources.Add(apiResource.ToEntity());

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var apiScope in Config.ApiScopes) context.ApiScopes.Add(apiScope.ToEntity());

                context.SaveChanges();
            }
        }
    }
}