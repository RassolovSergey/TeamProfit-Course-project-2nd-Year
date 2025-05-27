using AutoMapper;
using Data.Entities;
using Server.DTO.Product;
using Server.DTO.Reward;
using Server.Repositories.Interfaces;
using Server.Services.Interfaces;

public class RewardService : IRewardService
{
    private readonly IRewardRepository _rewardRepo;
    private readonly IUserProjectService _userProjectService;
    private readonly IProductRepository _productRepo;
    private readonly IMapper _mapper;

    public RewardService(
        IRewardRepository rewardRepo,
        IUserProjectService userProjectService,
        IProductRepository productRepo,
        IMapper mapper)
    {
        _rewardRepo = rewardRepo;
        _userProjectService = userProjectService;
        _productRepo = productRepo;
        _mapper = mapper;
    }

    public async Task<List<RewardDto>> GetByProjectAsync(int projectId, int userId)
    {
        var isMember = await _userProjectService.IsMemberAsync(projectId, userId);
        var isAdmin = await _userProjectService.IsAdminAsync(projectId, userId);
        if (!isMember && !isAdmin)
            throw new UnauthorizedAccessException("Access denied");

        var rewards = await _rewardRepo.GetAllAsync();
        var projectRewards = rewards.Where(r => r.ProjectId == projectId).ToList();

        return _mapper.Map<List<RewardDto>>(projectRewards);
    }

    public async Task<RewardDto> CreateRewardAsync(int projectId, CreateRewardDto dto, int userId)
    {
        var isAdmin = await _userProjectService.IsAdminAsync(projectId, userId);
        if (!isAdmin)
            throw new UnauthorizedAccessException("Access denied");

        var reward = _mapper.Map<Reward>(dto);
        reward.ProjectId = projectId;

        await _rewardRepo.AddAsync(reward);
        await _rewardRepo.SaveChangesAsync();

        return _mapper.Map<RewardDto>(reward);
    }

    public async Task<RewardDto?> UpdateRewardAsync(int rewardId, UpdateRewardDto dto, int userId)
    {
        var reward = await _rewardRepo.GetByIdAsync(rewardId);
        if (reward == null) return null;

        var isAdmin = await _userProjectService.IsAdminAsync(reward.ProjectId, userId);
        if (!isAdmin)
            throw new UnauthorizedAccessException("Access denied");

        _mapper.Map(dto, reward);
        await _rewardRepo.SaveChangesAsync();

        return _mapper.Map<RewardDto>(reward);
    }

    public async Task<bool> DeleteRewardAsync(int rewardId, int userId)
    {
        var reward = await _rewardRepo.GetByIdAsync(rewardId);
        if (reward == null) return false;

        var isAdmin = await _userProjectService.IsAdminAsync(reward.ProjectId, userId);
        if (!isAdmin)
            throw new UnauthorizedAccessException("Access denied");

        await _rewardRepo.DeleteAsync(rewardId);
        await _rewardRepo.SaveChangesAsync();

        return true;
    }

    public async Task<List<ProductDto>> GetProductsByRewardAsync(int rewardId, int userId)
    {
        var reward = await _rewardRepo.GetWithProductsAsync(rewardId);
        if (reward == null)
            throw new KeyNotFoundException("Reward not found");

        var isMember = await _userProjectService.IsMemberAsync(reward.ProjectId, userId);
        var isAdmin = await _userProjectService.IsAdminAsync(reward.ProjectId, userId);
        if (!isMember && !isAdmin)
            throw new UnauthorizedAccessException("Access denied");

        return _mapper.Map<List<ProductDto>>(reward.Products);
    }

    public async Task<ProductDto> AddProductToRewardAsync(int rewardId, CreateProductDto dto, int userId)
    {
        var reward = await _rewardRepo.GetWithProductsAsync(rewardId);
        if (reward == null)
            throw new KeyNotFoundException("Reward not found");

        var isAdmin = await _userProjectService.IsAdminAsync(reward.ProjectId, userId);
        if (!isAdmin)
            throw new UnauthorizedAccessException("Access denied");

        var product = _mapper.Map<Product>(dto);
        reward.Products.Add(product);

        await _rewardRepo.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto?> UpdateProductAsync(int productId, UpdateProductDto dto, int userId)
    {
        // Получаем продукт с включенными связями с наградами
        var product = await _productRepo.GetWithRewardsAsync(productId);
        if (product == null)
            return null;

        // Получаем первую награду из списка наград продукта
        var reward = product.Rewards.FirstOrDefault();
        if (reward == null)
            throw new KeyNotFoundException("Reward not found");

        // Проверяем, является ли пользователь администратором проекта, к которому принадлежит награда
        var isAdmin = await _userProjectService.IsAdminAsync(reward.ProjectId, userId);
        if (!isAdmin)
            throw new UnauthorizedAccessException("Access denied");

        // Маппим обновленные поля в продукт
        _mapper.Map(dto, product);
        await _productRepo.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }


    public async Task<bool> DeleteProductAsync(int productId, int userId)
    {
        var product = await _productRepo.GetWithRewardsAsync(productId);
        if (product == null)
            return false;

        var reward = product.Rewards.FirstOrDefault();
        if (reward == null)
            throw new KeyNotFoundException("Reward not found");

        var isAdmin = await _userProjectService.IsAdminAsync(reward.ProjectId, userId);
        if (!isAdmin)
            throw new UnauthorizedAccessException("Access denied");

        // Удаляем продукт
        await _productRepo.DeleteAsync(productId);
        await _productRepo.SaveChangesAsync();

        return true;
    }
}
