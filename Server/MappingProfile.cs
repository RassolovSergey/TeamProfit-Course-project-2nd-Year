using AutoMapper;
using Data.Entities;
using Server.DTO.Cost;
using Server.DTO.Currency.Server.DTO.Currency;
using Server.DTO.Currency;
using Server.DTO.Project;
using Server.DTO.User;
using Server.DTO.UserProject;
using Server.DTO.Reward;
using Server.DTO.Sale;
using Server.DTO.Product;

namespace Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User → Dto
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.HashPassword, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Costs, opt => opt.Ignore())
                .ForMember(dest => dest.Currency, opt => opt.Ignore());
            CreateMap<UpdateUserDto, User>()
                // не трогаем поля пароля и навигации
                .ForMember(dest => dest.HashPassword, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Costs, opt => opt.Ignore())
                .ForMember(dest => dest.Currency, opt => opt.Ignore());

            // --- Проект и участники проекта ---

            // 1) UserProject → UserProjectDto
            CreateMap<UserProject, UserProjectDto>();

            // 2) Project → ProjectDto (включая коллекцию участников)
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.UserProjects))
                .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => src.CurrencyId));

            // 3) CreateProjectDto → Project
            CreateMap<CreateProjectDto, Project>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.DateClose, opt => opt.Ignore())
                .ForMember(d => d.UserProjects, opt => opt.Ignore())
                .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => src.CurrencyId));

            // 4) UpdateProjectDto → Project
            CreateMap<UpdateProjectDto, Project>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.DateClose, opt => opt.Ignore())
                .ForMember(d => d.UserProjects, opt => opt.Ignore());

            // --- CRUD для UserProject ---

            // 5) CreateUserProjectDto → UserProject
            CreateMap<CreateUserProjectDto, UserProject>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.TypeCooperation, opt => opt.MapFrom(src => src.TypeCooperation))
                .ForMember(dest => dest.FixedPrice, opt => opt.MapFrom(src => src.FixedPrice))
                .ForMember(dest => dest.PercentPrice, opt => opt.MapFrom(src => src.PercentPrice));

            // 6) UpdateUserProjectDto → UserProject
            CreateMap<UpdateUserProjectDto, UserProject>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectId, opt => opt.Ignore())
                .ForMember(dest => dest.TypeCooperation, opt => opt.MapFrom(src => src.TypeCooperation))
                .ForMember(dest => dest.FixedPrice, opt => opt.MapFrom(src => src.FixedPrice))
                .ForMember(dest => dest.PercentPrice, opt => opt.MapFrom(src => src.PercentPrice));


            CreateMap<Cost, CostDto>();
            CreateMap<CreateCostDto, Cost>();
            CreateMap<UpdateCostDto, Cost>();


            // Валюты
            CreateMap<Currency, CurrencyDto>();
            CreateMap<CreateCurrencyDto, Currency>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.RateToBase, opt => opt.Ignore()); // курс заполняется системой
            CreateMap<UpdateCurrencyDto, Currency>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.RateToBase, opt => opt.Ignore()); // курс заполняется системой

            // Reward → RewardDto
            CreateMap<Reward, RewardDto>();
            CreateMap<Reward, RewardDto>();
            CreateMap<CreateRewardDto, Reward>().ForMember(d => d.Id, opt => opt.Ignore());
            CreateMap<UpdateRewardDto, Reward>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.ProjectId, opt => opt.Ignore());


            // В MappingProfile.cs в конструкторе:
            CreateMap<Sale, SaleDto>();
            CreateMap<CreateSaleDto, Sale>().ForMember(d => d.Id, opt => opt.Ignore());
            CreateMap<UpdateSaleDto, Sale>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.RewardId, opt => opt.Ignore());

            // Product → ProductDto
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.RewardIds, opt => opt.Ignore());

            // CreateProductDto → Product
            CreateMap<CreateProductDto, Product>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Rewards, opt => opt.Ignore());

            // UpdateProductDto → Product
            CreateMap<UpdateProductDto, Product>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Rewards, opt => opt.Ignore());
        }
    }
}
