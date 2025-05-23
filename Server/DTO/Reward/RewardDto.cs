using System.ComponentModel.DataAnnotations;

namespace Server.DTO.Reward
{
    /// <summary>DTO для возврата информации о награде</summary>
    public class RewardDto
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
