using System;
using System.Collections.Generic;
using System.Linq;

namespace BearStock.Dashboard.Entities
{
    public class Stock
    {
        internal Stock()
        {
        }

        public Stock(string ticket, int order, IEnumerable<StockPosition> stockPositions)
        {
            Ticket = ticket;
            Order = order;
            StockPositions = stockPositions.ToList();
        }

        public Guid Uuid { get; internal set; }
        public string Ticket { get; internal set; }
        public int Order { get; internal set; }
        public Dashboard Dashboard { get; internal set; }
        public Guid DashboardUuid { get; internal set; }

        public ICollection<StockPosition> StockPositions { get; internal set; } = new List<StockPosition>();
    }
}