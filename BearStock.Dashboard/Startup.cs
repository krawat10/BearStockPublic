using System;
using System.Threading.Tasks;
using AutoMapper;
using BearStock.Dashboard.Helpers;
using BearStock.Dashboard.Services;
using BearStock.Tools.Handlers;
using BearStock.Tools.Helpers;
using BearStock.Tools.Services;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace BearStock.Dashboard
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            services.AddSingleton(mappingConfig.CreateMapper());

            // services.AddDbContext<DataContext>(options =>
            // options.UseInMemoryDatabase(nameof(DataContext)));
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DataContext")));
            services.AddTransient<DashboardsService>();
            // use sql server db in production and sqlite db in development
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
            services.AddControllers();

            // configure strongly typed settings objects
            services.Configure<AuthSettings>(Configuration.GetSection("AuthSettings"));

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            // configure jwt authentication
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Authority = Configuration.GetValue<string>("AuthSettings:AuthorizationUrl"); // Auth URL
                    options.Audience = "SampleApp.API";

                    options.Events = new JwtBearerEvents() {
                        OnAuthenticationFailed = OnAuthenticationFailed, OnTokenValidated = OnTokenValidated
                    };
                });

            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("API");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private Task OnAuthenticationFailed(AuthenticationFailedContext arg)
        {
            Console.WriteLine("OnAuthenticationFailed");
            Console.WriteLine(arg.Exception.Message);
            Console.WriteLine(arg.Options?.Authority);
            Console.WriteLine(arg.Result?.Ticket.ToString());
            return Task.CompletedTask;
        }

        private Task OnTokenValidated(TokenValidatedContext arg)
        {
            Console.WriteLine("OnTokenValidated");

            return Task.CompletedTask;
        }
    }
}