using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(3)]
        public string Code { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(5)]
        public string? Symbol { get; set; }

        // Пользователи, выбравшие эту валюту
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
