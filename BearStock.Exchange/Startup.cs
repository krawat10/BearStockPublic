using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BearStock.Exchange.BackgroundServices;
using BearStock.Exchange.Helpers;
using BearStock.Tools.Helpers;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace BearStock.Exchange
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
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            services.AddSingleton(mappingConfig.CreateMapper());

            services.Configure<CryptoCurrenciesSettingsOptions>(Configuration.GetSection(CryptoCurrenciesSettingsOptions.Section));

            var apiSection = Configuration.GetSection(ApiSettingsOptions.Section);
            services.Configure<ApiSettingsOptions>(apiSection);
            services.AddHttpClient(apiSection.Get<ApiSettingsOptions>().APIName, c =>
            {
                c.BaseAddress = new Uri(apiSection.Get<ApiSettingsOptions>().APIUrl);
            });


            services.AddDbContext<DataContext>();
            services.AddHostedService<ExchangeRateBackgroundService>();

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

            services.Configure<AuthSettings>(Configuration.GetSection("AuthSettings"));

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            // configure jwt authentication
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Authority = Configuration.GetValue<string>("AuthSettings:AuthorizationUrl"); // Auth URL
                    options.ApiName = "SampleApp.API";
                });


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "BearStock.Exchange", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BearStock.Exchange v1"));
            }

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
    }
}