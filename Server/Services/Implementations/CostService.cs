using Server.DTO.Cost;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class CostService : ICostService
    {
        public Task<CostDto> CreateAsync(CreateCostDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CostDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CostDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CostDto?> UpdateAsync(int id, UpdateCostDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
