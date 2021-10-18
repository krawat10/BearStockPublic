using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BearStock.Exchange.DTO;
using BearStock.Exchange.Entities.Database;
using BearStock.Exchange.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BearStock.Exchange.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ExchangesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Exchanges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entities.Database.Exchange>>> GetExchanges()
        {
            return await _context.Exchanges.ToListAsync();
        }

        // GET: api/Exchanges/USD/BTC
        [HttpGet("{from}/{to}")]
        public async Task<ActionResult<ExchangeDTO>> GetExchange(string from, string to, DateTime? startTime)
        {
            startTime ??= DateTime.MinValue;

            var quotas = await _context.Quotas
                .Where(entity =>
                    entity.Exchange.FromCurrency == from &&
                    entity.Exchange.ToCurrency == to &&
                    entity.Date > startTime)
                .ToListAsync();


            if (!quotas.Any())
            {
                return NotFound();
            }

            return Ok(new ExchangeDTO {
                FromCurrency = @from,
                ToCurrency = to,
                Interval = IntervalType.Daily.ToString(),
                Quotas = _mapper.Map<IEnumerable<QuotaDTO>>(quotas)
            });
        }
    }
}