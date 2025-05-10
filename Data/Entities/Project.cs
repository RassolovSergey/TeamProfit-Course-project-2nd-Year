using System.ComponentModel.DataAnnotations;
using Data.Enums;

namespace Data.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;

        public DateTime? DateClose { get; set; }


        // Связь
        // Текущий статус кампании
        public ProjectStatus? Status { get; set; }

        // Команда (1:1)
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;

        // Вознаграждения и расходы
        public ICollection<Reward> Rewards { get; set; } = new List<Reward>();
        public ICollection<Cost> Costs { get; set; } = new List<Cost>();
    }
}
