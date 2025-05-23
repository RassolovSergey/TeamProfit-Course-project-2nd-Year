using Server.DTO.Cost;

namespace Server.Services.Interfaces
{
    public interface ICostService
    {
        Task<List<CostDto>> GetAllAsync();
        Task<CostDto?> GetByIdAsync(int id);
        Task<CostDto> CreateAsync(CreateCostDto dto);
        Task<CostDto?> UpdateAsync(int id, UpdateCostDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
