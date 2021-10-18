using System.ComponentModel.DataAnnotations;

namespace BearStock.Dashboard.DTOs
{
    public class DashboardDTO
    {
        [Required]

        public string Name { get; set; }
        [Required]
        public StockDTO[] Stocks  { get; set; }

        public bool IsDefault { get; set; }
    }
}