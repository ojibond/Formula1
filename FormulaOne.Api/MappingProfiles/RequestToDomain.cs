using AutoMapper;
using FormulaOne.Entities.DbSet;
using FormulaOne.Entities.Dtos.Requests;

namespace FormulaOne.Api.MappingProfiles
{
    public class RequestToDomain : Profile
    {
        public RequestToDomain()
        {
            CreateMap<CreateDriverAchievmentRequest, Achievment>()
                .ForMember(
                    dest => dest.RaceWins,
                    opt => opt.MapFrom(src => src.Wins))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => 1))
                .ForMember(
                    dest => dest.AddedDate,
                    opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(
                    dest => dest.UpdateDate,
                    opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UpdateDriverAchievmentRequest, Achievment>()
                .ForMember(
                    dest => dest.RaceWins,
                    opt => opt.MapFrom(src => src.Wins))
                .ForMember(
                    dest => dest.UpdateDate,
                    opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
