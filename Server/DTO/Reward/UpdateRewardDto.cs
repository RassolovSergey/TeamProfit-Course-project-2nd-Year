using System.ComponentModel.DataAnnotations;

namespace Server.DTO.Reward
{
    /// <summary>DTO для обновления награды</summary>
    public class UpdateRewardDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
