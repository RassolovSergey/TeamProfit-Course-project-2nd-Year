using Server.DTO.Currency;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class CurrencyService : ICurrencyService
    {
        public Task<CurrencyDto> CreateAsync(CreateCurrencyDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CurrencyDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CurrencyDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CurrencyDto?> UpdateAsync(int id, UpdateCurrencyDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
