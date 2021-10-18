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
using BearStock.Tools.Handlers;

namespace BearStock.FinancialModelingPrep
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

            var financialModelingPrepSettings =
                Configuration
                    .GetSection(nameof(FinancialModelingPrepSettings))
                    .Get<FinancialModelingPrepSettings>();

            services.AddSingleton(provider =>
                new ApiKeyHandler("apikey", financialModelingPrepSettings.ApiKey));

            services
                .AddHttpClient(FinancialModelingPrepSettings.ClientName,
                    client => client.BaseAddress = new Uri(financialModelingPrepSettings.Url))
                .AddHttpMessageHandler<ApiKeyHandler>()
                .AddTypedClient(Refit.RestService.For<IFinancialModelingPrepClient>);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("API");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}