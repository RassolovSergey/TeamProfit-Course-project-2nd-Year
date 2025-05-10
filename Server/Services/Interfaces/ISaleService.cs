using Server.DTO.Sale;

namespace Server.Services.Interfaces
{
    public interface ISaleService
    {
        /// <summary>Получить список всех команд</summary>
        Task<List<SaleDto>> GetAllAsync();

        /// <summary>Получить команду по идентификатору или null, если не найдена</summary>
        Task<SaleDto?> GetByIdAsync(int id);

        /// <summary>Создать новую команду и вернуть её DTO</summary>
        Task<SaleDto> CreateAsync(CreateSaleDto dto);

        /// <summary>Обновить существующую команду, вернуть обновлённый DTO или null, если не найдено</summary>
        Task<SaleDto?> UpdateAsync(int id, UpdateSaleDto dto);

        /// <summary>Удалить команду по идентификатору, вернуть true, если успешно</summary>
        Task<bool> DeleteAsync(int id);
    }
}
