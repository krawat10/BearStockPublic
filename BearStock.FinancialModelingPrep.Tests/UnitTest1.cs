using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BearStock.Tools.Handlers;
using NUnit.Framework;
using Refit;

namespace BearStock.FinancialModelingPrep.Tests
{
    public class FinancialModelingPrepClientTests
    {
        private IFinancialModelingPrepClient _restClient;

        [SetUp]
        public void Setup()
        {
            var httpClient =
                new HttpClient(new ApiKeyHandler("apikey", "XXX")) {
                    BaseAddress = new Uri("https://financialmodelingprep.com/api")
                };

            _restClient = RestService.For<IFinancialModelingPrepClient>(httpClient);
        }

        [Test]
        public async Task ShouldReturnStockList()
        {
            var list = await _restClient.GetStockList();

            Assert.NotNull(list);
            Assert.IsNotEmpty(list);
            Assert.IsNotEmpty(list.First().Name);
            Assert.IsNotEmpty(list.First().Exchange);
            Assert.IsNotEmpty(list.First().Symbol);
            Assert.NotZero(list.First().Price);
        }

        [Test]
        public async Task ShouldGetHistoricalPrice()
        {
            var historicalPriceFull = await _restClient.GetHistoricalPriceFull("GDX,INTC", "2018-03-12", "2018-03-13");

            Assert.NotNull(historicalPriceFull);


            Assert.IsNotEmpty(historicalPriceFull.HistoricalStockList);

            Assert.That(historicalPriceFull.HistoricalStockList, Has.One.With.Property("Symbol").EqualTo("GDX"));
            Assert.That(historicalPriceFull.HistoricalStockList, Has.One.With.Property("Symbol").EqualTo("INTC"));

            Assert.That(historicalPriceFull.HistoricalStockList,
                Has.All.With.Property("HistoricalList").Exactly(2).Items);

            Assert.That(historicalPriceFull.HistoricalStockList.SelectMany(stock => stock.HistoricalList),
                Has.All.Property("Open").GreaterThan(0d));

            Assert.That(historicalPriceFull.HistoricalStockList.SelectMany(stock => stock.HistoricalList),
                Has.All.Property("High").GreaterThan(0d));

            Assert.That(historicalPriceFull.HistoricalStockList.SelectMany(stock => stock.HistoricalList),
                Has.All.Property("Low").GreaterThan(0d));

            Assert.That(historicalPriceFull.HistoricalStockList.SelectMany(stock => stock.HistoricalList),
                Has.All.Property("Close").GreaterThan(0d));

            Assert.That(historicalPriceFull.HistoricalStockList.SelectMany(stock => stock.HistoricalList),
                Has.All.Property("AdjClose").GreaterThan(0d));
        }
    }
}