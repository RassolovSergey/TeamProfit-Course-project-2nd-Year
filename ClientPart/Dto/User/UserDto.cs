using Data.Enums;

namespace ClientPart.Dto.User
{
    /// <summary>
    /// Data Transfer Object для сущности User (выходная модель)
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string? Name { get; set; }
       public string? SurName { get; set; }
       public int? CurrencyId { get; set; }

    }
}
