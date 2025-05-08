using Server.DTO.Team;

namespace Server.Services.Interfaces
{
    /// <summary>
    /// Бизнес-логика для работы с командами
    /// </summary>
    public interface ITeamService
    {
        /// <summary>Получить список всех команд</summary>
        Task<List<TeamDto>> GetAllAsync();

        /// <summary>Получить команду по идентификатору или null, если не найдена</summary>
        Task<TeamDto?> GetByIdAsync(int id);

        /// <summary>Создать новую команду и вернуть её DTO</summary>
        Task<TeamDto> CreateAsync(CreateTeamDto dto);

        /// <summary>Обновить существующую команду, вернуть обновлённый DTO или null, если не найдено</summary>
        Task<TeamDto?> UpdateAsync(int id, UpdateTeamDto dto);

        /// <summary>Удалить команду по идентификатору, вернуть true, если успешно</summary>
        Task<bool> DeleteAsync(int id);
    }
}
