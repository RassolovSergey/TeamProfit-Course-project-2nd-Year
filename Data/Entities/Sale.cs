using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Sale
    {
        //Поля
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string BackerName { get; set; } = null!;

        [Required]
        public string BackerEmail { get; set; } = null!;


        // Связи
        // Вознаграждение
        public int RewardId { get; set; }
        public Reward Reward { get; set; } = null!;

    }
}
