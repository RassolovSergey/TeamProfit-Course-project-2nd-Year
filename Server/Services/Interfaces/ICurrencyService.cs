using Server.DTO.Currency;

namespace Server.Services.Interfaces
{
    public interface ICurrencyService
    {
        /// <summary>Получить список всех валют</summary>
        Task<List<CurrencyDto>> GetAllAsync();

        /// <summary>Получить валюту по идентификатору или null, если не найдена</summary>
        Task<CurrencyDto?> GetByIdAsync(int id);

        /// <summary>Создать новую валюту и вернуть её DTO</summary>
        Task<CurrencyDto> CreateAsync(CreateCurrencyDto dto);

        /// <summary>Обновить существующую валюту, вернуть обновлённый DTO или null, если не найдено</summary>
        Task<CurrencyDto?> UpdateAsync(int id, UpdateCurrencyDto dto);

        /// <summary>Удалить валюту по идентификатору, вернуть true, если успешно</summary>
        Task<bool> DeleteAsync(int id);
    }
}
