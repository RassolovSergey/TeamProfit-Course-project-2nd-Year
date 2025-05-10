using AutoMapper;
using Data.Entities;
using Server.DTO.User;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity → Dto
            CreateMap<User, UserDto>();

            // CreateDto → Entity
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.HashPassword, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Costs, opt => opt.Ignore())
                .ForMember(dest => dest.Teams, opt => opt.Ignore())
                .ForMember(dest => dest.Currency, opt => opt.Ignore());

            CreateMap<UpdateUserDto, User>()
                // не трогаем поля пароля и навигации
                .ForMember(dest => dest.HashPassword, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Costs, opt => opt.Ignore())
                .ForMember(dest => dest.Teams, opt => opt.Ignore())
                .ForMember(dest => dest.Currency, opt => opt.Ignore());
        }
    }
}
