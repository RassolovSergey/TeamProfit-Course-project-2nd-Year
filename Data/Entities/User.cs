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

        // Тип сотрудничества:
        // 0 - Фиксированная оплата
        // 1 - Процет от прибыли
        [Required]
        public TypeCooperation TypeCooperation { get; set; }
        public decimal? FixedPrice { get; set; }
        public byte? PercentPrice { get; set; }


        // Связи
        // Команды, в которых состоит пользователь
        public ICollection<Team> Teams { get; set; } = new List<Team>();



        // Основная валюта
        public int CurrencyId { get; set; }
        public Currency? Currency { get; set; }


        // Расходы пользователя
        public ICollection<Cost> Costs { get; set; } = new List<Cost>();
    }
}
