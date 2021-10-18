using AutoMapper;
using BearStock.Exchange.DTO;

namespace BearStock.Exchange.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Database.Quota, QuotaDTO>()
                .ForMember(dto => dto.O, expression => expression.MapFrom(quota => quota.Open))
                .ForMember(dto => dto.H, expression => expression.MapFrom(quota => quota.High))
                .ForMember(dto => dto.L, expression => expression.MapFrom(quota => quota.Low))
                .ForMember(dto => dto.C, expression => expression.MapFrom(quota => quota.Close));
        }
    }
}