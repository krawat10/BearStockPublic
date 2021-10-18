using System;

namespace BearStock.Dashboard.DTOs
{
    public class StockPositionDTO
    {
        public double PricePerShare { get; set; }
        public decimal TotalPrice { get; set; }
        public uint SharesAmount { get; set; }
        public DateTime Date { get; set; }
    }
}