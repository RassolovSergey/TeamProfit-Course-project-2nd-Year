using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Enums;

namespace Data.Entities
{
    public class Cost
    {
        // Поля
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public CostType Type { get; set; } = CostType.ProfitPercentage;

        // Связи
        // Проект
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        // Пользователь
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
