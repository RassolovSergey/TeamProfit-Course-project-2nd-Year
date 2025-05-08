using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Product
    {
        // Поля
        [Key]
        public int Id { get; set; }


        [Required]
        public string Name { get; set; } = null!;

        // Связи
        // Вознаграждения (многие-ко-многим)
        public ICollection<Reward> Rewards { get; set; } = new List<Reward>();
    }
}
