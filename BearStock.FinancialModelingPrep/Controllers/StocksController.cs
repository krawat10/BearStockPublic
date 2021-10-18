using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BearStock.FinancialModelingPrep.Controllers
{
    public class ChartDTO
    {
        public decimal BookValue { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public decimal PE { get; set; }
        public string Sector { get; set; }
        public IEnumerable<StockPrice> Values { get; set; }

        public class StockPrice
        {
            public string D { get; set; }
            public decimal O { get; set; }
            public decimal H { get; set; }
            public decimal L { get; set; }
            public decimal C { get; set; }

        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IFinancialModelingPrepClient _modelingPrepClient;

        public StocksController(IFinancialModelingPrepClient modelingPrepClient)
        {
            _modelingPrepClient = modelingPrepClient;
        }

        // GET api/<StocksController>/5
        [HttpGet("{ticket}")]
        public async Task<IActionResult> Get(string ticket)
        {
            var stock = await _modelingPrepClient.GetHistoricalPriceFull(ticket,
                DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd"),
                DateTime.Now.ToString("yyyy-MM-dd"));

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(new ChartDTO {
                Currency = "USD",
                BookValue = 0,
                Name = stock.Symbol,
                PE = 0,
                Sector = "",
                Values = stock.HistoricalList.Select(historical => new ChartDTO.StockPrice {
                    O = historical.Open,
                    H = historical.High,
                    L = historical.Low,
                    C = historical.Close,
                    D = historical.Date
                })
            });
        }
    }
}