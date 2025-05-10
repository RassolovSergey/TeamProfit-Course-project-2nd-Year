using Data.Enums;

namespace Server.DTO.User
{
    /// <summary>
    /// Data Transfer Object для сущности User (выходная модель)
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Data.Enums.TypeCooperation TypeCooperation { get; set; }
    }
}
