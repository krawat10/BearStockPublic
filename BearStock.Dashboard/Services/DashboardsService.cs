using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BearStock.Dashboard.DTOs;
using BearStock.Dashboard.Entities;
using BearStock.Dashboard.Helpers;
using BearStock.Tools.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BearStock.Dashboard.Services
{
    public class DashboardsService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DashboardsService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteDashboards(string name, string userId)
        {
            var dashboard = await _context.Dashboards.FirstOrDefaultAsync(entity =>
                entity.Name == name && entity.UserId == userId);

            if (dashboard == null)
            {
                throw new ObjectNotFoundException(name);
            }

            _context.Dashboards.Remove(dashboard);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DashboardDTO>> GetDashboards(string userId)
        {
            var dashboards = await _context.Dashboards
                .Include(dashboard => dashboard.Stocks)
                .ThenInclude(stock => stock.StockPositions)
                .Where(dashboard => dashboard.UserId == userId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<DashboardDTO>>(dashboards);
        }


        public async Task<DashboardDTO> GetDashboard(string name, string userId)
        {
            var dashboard = await _context.Dashboards
                .FirstOrDefaultAsync(entity => entity.UserId == userId && entity.Name == name);

            if (dashboard == null) throw new ObjectNotFoundException(name);

            return _mapper.Map<DashboardDTO>(dashboard);
        }

        public async Task AddDashboard(DashboardDTO dto, string userId)
        {
            bool exists = await _context.Dashboards.AnyAsync(dashboard =>
                dashboard.Name == dto.Name && dashboard.UserId == userId);

            if (exists) throw new ObjectDuplicateException(dto.Name);

            await _context.Dashboards.AddAsync(new Entities.Dashboard {
                Name = dto.Name,
                IsDefault = dto.IsDefault,
                UserId = userId,
                Stocks = dto.Stocks
                    .Select((chartDTO, idx) => new Stock(
                        chartDTO.Ticket,
                        idx,
                        chartDTO.StockPositions.Select(positionDTO => new StockPosition(
                            positionDTO.PricePerShare,
                            positionDTO.TotalPrice,
                            positionDTO.SharesAmount,
                            positionDTO.Date))))
                    .ToList()
            });

            await _context.SaveChangesAsync();
        }

        public async Task UpdateDashboard(DashboardDTO dto, string userId)
        {
            var entity = await _context.Dashboards
                .Include(d => d.Stocks)
                .ThenInclude(stocks => stocks.StockPositions)
                .FirstOrDefaultAsync(x => x.Name == dto.Name && x.UserId == userId);

            if (entity == null) throw new ObjectNotFoundException(dto.Name);

            entity.IsDefault = dto.IsDefault;
            _context.Stocks.RemoveRange(entity.Stocks);

            entity.Stocks = dto.Stocks
                .Select((chartDTO, idx) => new Stock(
                    chartDTO.Ticket,
                    idx,
                    chartDTO.StockPositions.Select(positionDTO => new StockPosition(
                        positionDTO.PricePerShare,
                        positionDTO.TotalPrice,
                        positionDTO.SharesAmount,
                        positionDTO.Date))))
                .ToList();

            await _context.SaveChangesAsync();
        }
    }
}