using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BearStock.Exchange.Entities.Api
{
    public class HistoricalExchangeRate
    {
        [JsonPropertyName("Meta Data")] public MetaData MetaData { get; set; }

        [JsonPropertyName("Time Series (Digital Currency Daily)")]
        public Dictionary<DateTime, Dictionary<string, string>> TimeSeriesDigitalCurrencyDaily { get; set; }
    }

    public class MetaData
    {
        [JsonPropertyName("1. Information")] public string The1Information { get; set; }

        [JsonPropertyName("2. Digital Currency Code")]
        public string The2DigitalCurrencyCode { get; set; }

        [JsonPropertyName("3. Digital Currency Name")]
        public string The3DigitalCurrencyName { get; set; }

        [JsonPropertyName("4. Market Code")] public string The4MarketCode { get; set; }

        [JsonPropertyName("5. Market Name")] public string The5MarketName { get; set; }

        [JsonPropertyName("6. Last Refreshed")] public string The6LastRefreshed { get; set; }

        [JsonPropertyName("7. Time Zone")] public string The7TimeZone { get; set; }
    }
}
