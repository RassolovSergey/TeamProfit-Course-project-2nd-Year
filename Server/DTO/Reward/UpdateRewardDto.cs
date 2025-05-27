using System.ComponentModel.DataAnnotations;

namespace Server.DTO.Reward
{
    /// <summary>DTO для обновления награды</summary>
    public class UpdateRewardDto
    {
        [StringLength(50)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public decimal? Price { get; set; }
    }
}
