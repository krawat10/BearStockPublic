using System;
using System.Collections.Generic;

namespace BearStock.Dashboard.Entities
{
    public class Dashboard 
    {
        public Guid Uuid { get; set; }
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
        public string Name { get; set; }
        public string UserId { get; set; }
        public bool IsDefault { get; set; }
    }
}