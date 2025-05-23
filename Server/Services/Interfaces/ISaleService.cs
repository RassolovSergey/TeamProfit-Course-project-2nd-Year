using Server.DTO.Sale;

namespace Server.Services.Interfaces
{
    public interface ISaleService
    {
        Task<List<SaleDto>> GetAllAsync();
        Task<List<SaleDto>> GetByProjectAsync(int projectId);
        Task<SaleDto?> GetByIdAsync(int id);
        Task<SaleDto> CreateAsync(CreateSaleDto dto, int rewardId);
        Task<SaleDto?> UpdateAsync(int id, UpdateSaleDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
