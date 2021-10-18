using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Refit;

namespace BearStock.FinancialModelingPrep
{
    public class StockListItem
    {
        [JsonPropertyName("symbol")] public string Symbol { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("price")] public decimal Price { get; set; }
        [JsonPropertyName("exchange")] public string Exchange { get; set; }
    }

    public class HistoricalPriceFull
    {
        [JsonPropertyName("historicalStockList")]
        public IEnumerable<HistoricalStock> HistoricalStockList { get; set; }

        public class HistoricalStock
        {
            [JsonPropertyName("symbol")] public string Symbol { get; set; }

            [JsonPropertyName("historical")] public IEnumerable<Historical> HistoricalList { get; set; }

            public class Historical
            {
                [JsonPropertyName("date")] public string Date { get; set; }

                [JsonPropertyName("open")] public decimal Open { get; set; }

                [JsonPropertyName("high")] public decimal High { get; set; }

                [JsonPropertyName("low")] public decimal Low { get; set; }

                [JsonPropertyName("close")] public decimal Close { get; set; }

                [JsonPropertyName("adjClose")] public double AdjClose { get; set; }

                [JsonPropertyName("volume")] public double Volume { get; set; }

                [JsonPropertyName("unadjustedVolume")] public double UnadjustedVolume { get; set; }

                [JsonPropertyName("change")] public double Change { get; set; }

                [JsonPropertyName("changePercent")] public double ChangePercent { get; set; }

                [JsonPropertyName("vwap")] public double Vwap { get; set; }

                [JsonPropertyName("label")] public string Label { get; set; }

                [JsonPropertyName("changeOverTime")] public double ChangeOverTime { get; set; }
            }
        }
    }

    public interface IFinancialModelingPrepClient
    {
        [Get(
            "/v3/historical-price-full/{**symbols}")]
        Task<HistoricalPriceFull> GetHistoricalPriceFullList([AliasAs("symbols")] string symbols, string from,
            string to);
        [Get(
            "/v3/historical-price-full/{**symbol}")]
        Task<HistoricalPriceFull.HistoricalStock> GetHistoricalPriceFull([AliasAs("symbol")] string symbol, string from,
            string to);

        [Get("/v3/stock/list")]
        Task<IEnumerable<StockListItem>> GetStockList();
    }
}