using AutoMapper;
using Common.Dto;
using Repository.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TransactionDto, Transaction>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => Enum.Parse<TransactionStatus>(src.Status, true)));

            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}