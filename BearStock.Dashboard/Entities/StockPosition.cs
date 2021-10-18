using System;

namespace BearStock.Dashboard.Entities
{
    public class StockPosition
    {
        internal StockPosition()
        {
        }

        public StockPosition(double pricePerShare, decimal totalPrice, uint sharesAmount, DateTime date)
        {
            PricePerShare = pricePerShare;
            TotalPrice = totalPrice;
            SharesAmount = sharesAmount;
            Date = date;
        }

        public Guid Uuid { get; internal set; }
        public Stock Stock { get; internal set; }
        public Guid StockUuid { get; internal set; }

        public double PricePerShare { get; internal set; }
        public decimal TotalPrice { get; internal set; }
        public uint SharesAmount { get; internal set; }
        public DateTime Date { get; internal set; }
    }
}