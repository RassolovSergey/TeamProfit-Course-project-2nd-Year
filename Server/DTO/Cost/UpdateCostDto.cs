using Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Server.DTO.Cost
{
    public class UpdateCostDto
    {
        [Required]
        public decimal Amount { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public CostType Type { get; set; }
    }
}
