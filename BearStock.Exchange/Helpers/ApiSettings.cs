using System.Collections.Generic;

namespace BearStock.Exchange.Helpers
{
    public class ApiSettingsOptions
    {
        public const string Section = "ApiSettings";

        public string APIName { get; set; } //"alphavantage"
        public string APIKey { get; set; }
        public string APIUrl { get; set; }
    }

    public class CryptoCurrenciesSettingsOptions
    {
        public const string Section = "CryptoCurrenciesSettings";

        public string Market { get; set; }
        public IDictionary<string, string> CryptoCurrencies { get; set; }
    }
}