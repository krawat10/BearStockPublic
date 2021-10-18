using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BearStock.Exchange.Entities.Database
{
    public enum IntervalType
    {
        Daily
    }

    public enum CurrencyType
    {
        Currency,
        CryptoCurrency
    }

    public class Exchange
    {
        [Key] public long UuId { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public IntervalType Interval { get; set; }
        public CurrencyType Currency { get; set; }
        public IEnumerable<Quota> Quotas { get; set; }
        public bool IsObsolete { get; set; }
    }
}