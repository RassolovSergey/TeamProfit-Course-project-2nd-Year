using AutoMapper;
using Data.Entities;
using Server.DTO.Cost;
using Server.DTO.Currency;
using Server.DTO.Currency.Server.DTO.Currency;
using Server.DTO.Product;
using Server.DTO.Project;
using Server.DTO.Reward;
using Server.DTO.Sale;
using Server.DTO.User;
using Server.DTO.UserProject;

namespace Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ----------------------------------------------------------------
            // 1. Пользователь
            // ----------------------------------------------------------------

            // 1.1 User → UserDto
            CreateMap<User, UserDto>();

            // 1.2 CreateUserDto → User (пароль соль и навигацию игнорируем)
            CreateMap<CreateUserDto, User>()
                .ForMember(d => d.HashPassword, o => o.Ignore())
                .ForMember(d => d.PasswordSalt, o => o.Ignore())
                .ForMember(d => d.Costs, o => o.Ignore())
                .ForMember(d => d.Currency, o => o.Ignore());

            // 1.3 UpdateUserDto → User (меняем только поля профиля, пароль и навигацию игнорируем)
            CreateMap<UpdateUserDto, User>()
                .ForMember(d => d.HashPassword, o => o.Ignore())
                .ForMember(d => d.PasswordSalt, o => o.Ignore())
                .ForMember(d => d.Costs, o => o.Ignore())
                .ForMember(d => d.Currency, o => o.Ignore());

            // ----------------------------------------------------------------
            // 2. Проект и привязки «пользователь — проект»
            // ----------------------------------------------------------------

            // 2.1 UserProject → UserProjectDto
            CreateMap<UserProject, UserProjectDto>();

            // 2.2 Project → ProjectDto (+ собираем коллекцию участников и берём CurrencyId)
            CreateMap<Project, ProjectDto>()
                .ForMember(d => d.Participants, o => o.MapFrom(src => src.UserProjects))
                .ForMember(d => d.CurrencyId, o => o.MapFrom(src => src.CurrencyId));

            // 2.3 CreateProjectDto → Project
            //      все навигации, Id, DateClose и CreatorUserId игнорируем,
            //      CurrencyId берётся из DTO
            CreateMap<CreateProjectDto, Project>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.DateClose, o => o.Ignore())
                .ForMember(d => d.UserProjects, o => o.Ignore())
                .ForMember(d => d.CurrencyId, o => o.MapFrom(src => src.CurrencyId));

            // 2.4 UpdateProjectDto → Project
            CreateMap<UpdateProjectDto, Project>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.DateClose, o => o.Ignore())
                .ForMember(d => d.UserProjects, o => o.Ignore());

            // ----------------------------------------------------------------
            // 3. CRUD для привязок UserProject
            // ----------------------------------------------------------------

            // 3.1 CreateUserProjectDto → UserProject
            CreateMap<CreateUserProjectDto, UserProject>()
                .ForMember(d => d.UserId, o => o.MapFrom(src => src.UserId))
                .ForMember(d => d.TypeCooperation, o => o.MapFrom(src => src.TypeCooperation))
                .ForMember(d => d.FixedPrice, o => o.MapFrom(src => src.FixedPrice))
                .ForMember(d => d.PercentPrice, o => o.MapFrom(src => src.PercentPrice));

            // 3.2 UpdateUserProjectDto → UserProject (идентификаторы игнорируем)
            CreateMap<UpdateUserProjectDto, UserProject>()
                .ForMember(d => d.UserId, o => o.Ignore())
                .ForMember(d => d.ProjectId, o => o.Ignore())
                .ForMember(d => d.TypeCooperation, o => o.MapFrom(src => src.TypeCooperation))
                .ForMember(d => d.FixedPrice, o => o.MapFrom(src => src.FixedPrice))
                .ForMember(d => d.PercentPrice, o => o.MapFrom(src => src.PercentPrice));

            // ----------------------------------------------------------------
            // 4. Затраты (Costs)
            // ----------------------------------------------------------------

            CreateMap<Cost, CostDto>();
            CreateMap<CreateCostDto, Cost>();
            CreateMap<UpdateCostDto, Cost>();

            // ----------------------------------------------------------------
            // 5. Валюты (Currencies)
            // ----------------------------------------------------------------

            CreateMap<Currency, CurrencyDto>();
            CreateMap<CreateCurrencyDto, Currency>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.RateToBase, o => o.Ignore()); // обновляется сервисом
            CreateMap<UpdateCurrencyDto, Currency>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.RateToBase, o => o.Ignore());

            // ----------------------------------------------------------------
            // 6. Вознаграждения (Rewards)
            // ----------------------------------------------------------------

            CreateMap<Reward, RewardDto>();
            CreateMap<CreateRewardDto, Reward>()
                .ForMember(d => d.Id, o => o.Ignore());
            CreateMap<UpdateRewardDto, Reward>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.ProjectId, o => o.Ignore());

            // ----------------------------------------------------------------
            // 7. Продажи (Sales)
            // ----------------------------------------------------------------

            CreateMap<Sale, SaleDto>();
            CreateMap<CreateSaleDto, Sale>()
                .ForMember(d => d.Id, o => o.Ignore());
            CreateMap<UpdateSaleDto, Sale>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.RewardId, o => o.Ignore());

            // ----------------------------------------------------------------
            // 8. Продукты (Products)
            // ----------------------------------------------------------------

            CreateMap<Product, ProductDto>()
                .ForMember(d => d.RewardIds, o => o.Ignore());
            CreateMap<CreateProductDto, Product>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Rewards, o => o.Ignore());
            CreateMap<UpdateProductDto, Product>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Rewards, o => o.Ignore());
        }
    }
}
