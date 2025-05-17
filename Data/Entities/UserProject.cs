using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities
{
    /// <summary>
    /// Связь «пользователь ↔ проект» с условиями сотрудничества и флагом администратора
    /// </summary>
    public class UserProject
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [Key, Column(Order = 1)]
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        /// <summary>Тип сотрудничества (enum)</summary>
        [Required]
        public TypeCooperation TypeCooperation { get; set; }

        /// <summary>Фиксированная сумма вознаграждения</summary>
        [Required]
        public decimal FixedPrice { get; set; }

        /// <summary>Процент от прибыли</summary>
        [Required]
        public decimal PercentPrice { get; set; }

        /// <summary>Флаг администратора проекта</summary>
        [Required]
        public bool IsAdmin { get; set; }
    }
}
