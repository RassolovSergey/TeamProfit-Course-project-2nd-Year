using System.ComponentModel.DataAnnotations;
using Data.Enums;

namespace Data.Entities
{
    public class User
    {
        // Поля
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string Login { get; set; } = null!;
        [MaxLength(40)]
        public string? Name { get; set; }

        [MaxLength(40)]
        public string? SurName { get; set; }

        [Required, MaxLength(40)]
        public string Email { get; set; } = null!;

        [Required]
        public string HashPassword { get; set; } = null!;

        [Required]
        public string PasswordSalt { get; set; } = null!;



        // Связи
        // Команды, в которых состоит пользователь
        public ICollection<Team> Teams { get; set; } = new List<Team>();



        // Основная валюта
        public int? CurrencyId { get; set; }
        public Currency? Currency { get; set; }


        // Расходы пользователя
        public ICollection<Cost> Costs { get; set; } = new List<Cost>();

        // Параметры сотрудничества по проектам
        public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
    }
}
