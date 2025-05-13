using System.ComponentModel.DataAnnotations;
using Data.Enums;

namespace Data.Entities
{
    public class UserProject
    {
        // Составной ключ: UserId + ProjectId
        public int UserId { get; set; }
        public int ProjectId { get; set; }

        // Поля-параметры сотрудничества для данного проекта
        [Required]
        public TypeCooperation TypeCooperation { get; set; }

        public decimal? FixedPrice { get; set; }
        public byte? PercentPrice { get; set; }

        // Навигационные свойства
        public User User { get; set; } = null!;
        public Project Project { get; set; } = null!;
    }
}
