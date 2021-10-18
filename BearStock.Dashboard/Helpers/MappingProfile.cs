using System.Linq;
using AutoMapper;
using BearStock.Dashboard.DTOs;
using BearStock.Dashboard.Entities;

namespace BearStock.Dashboard.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Stock, StockDTO>();
            CreateMap<Entities.StockPosition, StockPositionDTO>();
            CreateMap<Entities.Dashboard, DashboardDTO>();
        }
    }
}