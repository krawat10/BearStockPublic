using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BearStock.Dashboard.DTOs
{
    public class StockDTO
    {
        [Required]
        public string Ticket { get; set; }
        public int Order { get; set; }
        public IEnumerable<StockPositionDTO> StockPositions { get; set; }
    }
}