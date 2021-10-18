using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using BearStock.Exchange.Entities.Api;
using BearStock.Exchange.Entities.Database;
using BearStock.Exchange.Helpers;
using BearStock.Tools.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BearStock.Exchange.BackgroundServices
{
    public class ExchangeRateBackgroundService : BackgroundService
    {
        private readonly ILogger<ExchangeRateBackgroundService> _logger;
        private readonly ApiSettingsOptions _apiSettings;
        private CryptoCurrenciesSettingsOptions _cryptoCurrenciesSettings;

        public ExchangeRateBackgroundService(IServiceProvider services,
            ILogger<ExchangeRateBackgroundService> logger,
            IOptions<ApiSettingsOptions> apiSettings,
            IOptions<CryptoCurrenciesSettingsOptions> cryptoCurrenciesSettings
        )
        {
            Services = services;
            _logger = logger;
            _apiSettings = apiSettings.Value;
            _cryptoCurrenciesSettings = cryptoCurrenciesSettings.Value;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            while (true)
            {
                using var scope = Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var clientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();

                foreach (var cryptoCurrency in _cryptoCurrenciesSettings.CryptoCurrencies)
                {
                    _logger.LogInformation($"Fetching daily data for {cryptoCurrency.Value}.");

                    var exchange = context.Exchanges
                        .FirstOrDefault(ex =>
                            ex.FromCurrency == cryptoCurrency.Key &&
                            ex.ToCurrency == _cryptoCurrenciesSettings.Market);

                    if (exchange == null)
                    {
                        exchange = new Entities.Database.Exchange {
                            FromCurrency = cryptoCurrency.Key,
                            ToCurrency = _cryptoCurrenciesSettings.Market,
                            Interval = IntervalType.Daily,
                            Currency = CurrencyType.CryptoCurrency
                        };
                        await context.AddAsync(exchange, stoppingToken);
                    }

                    try
                    {
                        if (await ShouldBeUpdated(exchange, context))
                        {
                            var httpClient = clientFactory.CreateClient(_apiSettings.APIName);
                            HistoricalExchangeRate exchangeRate = null;

                            if (exchange.Currency == CurrencyType.CryptoCurrency)
                            {
                                exchangeRate = await httpClient
                                    .GetFromJsonAsync<HistoricalExchangeRate>("query?" +
                                                                              $"function=DIGITAL_CURRENCY_DAILY&" +
                                                                              $"market={exchange.ToCurrency}&" +
                                                                              $"symbol={exchange.FromCurrency}&" +
                                                                              $"apikey={_apiSettings.APIKey}",
                                        cancellationToken: stoppingToken);
                            }

                            if (exchangeRate.TimeSeriesDigitalCurrencyDaily == null)
                            {
                                exchange.IsObsolete = true;
                            }
                            else
                            {
                                var lastQuotaDate = await context.Quotas.Where(quota =>
                                                            quota.Exchange.FromCurrency == exchange.FromCurrency &&
                                                            quota.Exchange.ToCurrency == exchange.ToCurrency)
                                                        .MaxAsync(quota => (DateTime?)quota.Date, stoppingToken) ??
                                                    DateTime.MinValue;

                                var newQuotas = exchangeRate
                                    .TimeSeriesDigitalCurrencyDaily
                                    .Where(x => x.Key > lastQuotaDate)
                                    .Select(x => new Quota {
                                        Date = x.Key,
                                        Open =
                                            decimal.Parse(x.Value.GetValueByKeyContains("1a."),
                                                CultureInfo.InvariantCulture),
                                        High =
                                            decimal.Parse(x.Value.GetValueByKeyContains("2a."),
                                                CultureInfo.InvariantCulture),
                                        Low = decimal.Parse(x.Value.GetValueByKeyContains("3a."),
                                            CultureInfo.InvariantCulture),
                                        Close = decimal.Parse(x.Value.GetValueByKeyContains("4a."), CultureInfo.InvariantCulture),
                                        Exchange = exchange
                                    })
                                    .ToList();

                                await context.Quotas.AddRangeAsync(newQuotas, stoppingToken);
                            }

                            await context.SaveChangesAsync(stoppingToken);
                            await Task.Delay(60 / 5 * 1000, stoppingToken);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                    }
                }
            }
        }

        private async Task<bool> ShouldBeUpdated(Entities.Database.Exchange exchange, DataContext context)
        {
            if (exchange.IsObsolete) return false;

            switch (exchange.Interval)
            {
                case IntervalType.Daily:
                {
                    var today = DateTime.Now.Date;

                    return !await context.Quotas
                        .AnyAsync(quota =>
                            quota.Exchange == exchange &&
                            quota.Date.Year == today.Year &&
                            quota.Date.Month == today.Month &&
                            quota.Date.Day == today.Day);
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}