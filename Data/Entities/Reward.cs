using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Reward
    {
        // Поля
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        // Проект
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
