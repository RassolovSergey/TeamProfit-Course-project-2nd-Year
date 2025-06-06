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
    private readonly ILogger<RewardService> _logger;


    public RewardService(
        IRewardRepository rewardRepo,
        IUserProjectService userProjectService,
        IProductRepository productRepo,
        IMapper mapper,
        ILogger<RewardService> logger)
    {
        _rewardRepo = rewardRepo;
        _userProjectService = userProjectService;
        _productRepo = productRepo;
        _mapper = mapper;
        _logger = logger;
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

    public async Task<Product?> GetProductWithRewardsAsync(int productId)
    {
        return await _productRepo.GetWithRewardsAsync(productId);
    }


    public async Task<RewardDto?> GetByIdAsync(int rewardId)
    {
        var reward = await _rewardRepo.GetByIdAsync(rewardId);
        return reward == null ? null : _mapper.Map<RewardDto>(reward);
    }

    public async Task<Reward> GetEntityByIdAsync(int rewardId)
    {
        var reward = await _rewardRepo.GetByIdAsync(rewardId);
        if (reward == null)
            throw new KeyNotFoundException("Reward not found");
        return reward;
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

    public async Task<bool> DeleteProductByIdAsync(int productId, int userId)
    {
        var product = await _productRepo.GetWithRewardsAsync(productId);
        if (product == null)
            return false;

        var adminReward = product.Rewards.FirstOrDefault(r =>
            _userProjectService.IsAdminAsync(r.ProjectId, userId).GetAwaiter().GetResult());

        if (adminReward == null)
            throw new UnauthorizedAccessException("Access denied");

        adminReward.Products.Remove(product);

        // Явное удаление продукта из БД
        await _productRepo.DeleteAsync(product.Id);

        await _rewardRepo.SaveChangesAsync();

        return true;
    }



    public async Task<ProductDto> AddProductToRewardAsync(int rewardId, CreateProductDto dto, int userId)
    {
        var reward = await _rewardRepo.GetWithProductsAsync(rewardId);

        if (reward == null)
            throw new UnauthorizedAccessException("Access denied or reward not found");

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


    public async Task<bool> DeleteProductFromRewardAsync(int rewardId, int productId, int userId)
    {
        _logger.LogInformation("DeleteProductFromRewardAsync called with rewardId={RewardId}, productId={ProductId}, userId={UserId}", rewardId, productId, userId);

        // 1) Подгрузить вознаграждение вместе с продуктами
        var reward = await _rewardRepo.GetWithProductsAsync(rewardId);
        if (reward == null)
        {
            _logger.LogWarning("Reward with id {RewardId} not found", rewardId);
            return false;
        }

        _logger.LogInformation("Found reward with projectId={ProjectId}", reward.ProjectId);

        // 2) Проверить, что пользователь — админ этого проекта
        var isAdmin = await _userProjectService.IsAdminAsync(reward.ProjectId, userId);
        _logger.LogInformation("Is user with id {UserId} admin of project {ProjectId}: {IsAdmin}", userId, reward.ProjectId, isAdmin);

        if (!isAdmin)
        {
            _logger.LogWarning("Access denied for user {UserId} to delete product from reward {RewardId}", userId, rewardId);
            throw new UnauthorizedAccessException("Access denied");
        }

        // 3) Найти продукт внутри этого вознаграждения
        var product = reward.Products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            _logger.LogWarning("Product with id {ProductId} not found in reward {RewardId}", productId, rewardId);
            return false;
        }

        // 4) Убрать связь Product ↔ Reward
        reward.Products.Remove(product);

        // 5) Сохранить изменения
        await _rewardRepo.SaveChangesAsync();
        _logger.LogInformation("Product with id {ProductId} removed from reward {RewardId}", productId, rewardId);

        return true;
    }


}
