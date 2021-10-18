using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BearStock.Exchange.Entities.Database
{
    public class Quota
    {
        [Key] public long Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public Exchange Exchange { get; set; }
        [ForeignKey(nameof(Exchange))] public long ExchangeId { get; set; }
    }
}