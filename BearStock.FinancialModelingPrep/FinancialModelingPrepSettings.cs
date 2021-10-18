using Microsoft.Extensions.Options;

namespace BearStock.FinancialModelingPrep
{
    public class FinancialModelingPrepSettings
    {
        public static string ClientName => "FinancialModelingPrep";
        public string Url { get; set; }
        public string ApiKey { get; set; }
    }
}