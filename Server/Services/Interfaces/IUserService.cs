using Server.DTO.User;

namespace Server.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>Получить список всех команд</summary>
        Task<List<UserDto>> GetAllAsync();

        /// <summary>Получить команду по идентификатору или null, если не найдена</summary>
        Task<UserDto?> GetByIdAsync(int id);

        /// <summary>Создать новую команду и вернуть её DTO</summary>
        Task<UserDto> CreateAsync(CreateUserDto dto);

        /// <summary>Обновить существующую команду, вернуть обновлённый DTO или null, если не найдено</summary>
        Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto);

        /// <summary>Удалить команду по идентификатору, вернуть true, если успешно</summary>
        Task<bool> DeleteAsync(int id);
    }
}
