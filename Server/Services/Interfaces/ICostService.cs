using Server.DTO.Cost;

namespace Server.Services.Interfaces
{
    public interface ICostService
    {
        /// <summary>Получить список всех затрат</summary>
        Task<List<CostDto>> GetAllAsync();

        /// <summary>Получить затрату по идентификатору или null, если не найдена</summary>
        Task<CostDto?> GetByIdAsync(int id);

        /// <summary>Создать новую затрату и вернуть её DTO</summary>
        Task<CostDto> CreateAsync(CreateCostDto dto);

        /// <summary>Обновить существующую затрату, вернуть обновлённый DTO или null, если не найдено</summary>
        Task<CostDto?> UpdateAsync(int id, UpdateCostDto dto);

        /// <summary>Удалить затрату по идентификатору, вернуть true, если успешно</summary>
        Task<bool> DeleteAsync(int id);
    }
}
