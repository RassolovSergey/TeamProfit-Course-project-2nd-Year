using Server.DTO.Currency;
using Server.DTO.Currency.Server.DTO.Currency;

namespace Server.Services.Interfaces
{
    /// <summary>
    /// Сервис для бизнес-логики, связанной с валютами
    /// </summary>
    public interface ICurrencyService
    {
        Task<List<CurrencyDto>> GetAllAsync();
        Task<CurrencyDto?> GetByIdAsync(int id);
        Task<CurrencyDto> CreateAsync(CreateCurrencyDto dto);
        Task<CurrencyDto?> UpdateAsync(int id, UpdateCurrencyDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
