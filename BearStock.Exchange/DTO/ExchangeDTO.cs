using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BearStock.Exchange.Entities.Database;

namespace BearStock.Exchange.DTO
{
    public class ExchangeDTO
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public string Interval { get; set; }
        public IEnumerable<QuotaDTO> Quotas { get; set; }
    }

    public class QuotaDTO
    {
        public DateTime Date { get; set; }
        public decimal O { get; set; }
        public decimal H { get; set; }
        public decimal L { get; set; }
        public decimal C { get; set; }

    }
}