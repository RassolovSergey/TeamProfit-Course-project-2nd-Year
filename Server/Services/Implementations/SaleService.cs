using Server.DTO.Sale;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class SaleService : ISaleService
    {
        public Task<SaleDto> CreateAsync(CreateSaleDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<SaleDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SaleDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<SaleDto?> UpdateAsync(int id, UpdateSaleDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
