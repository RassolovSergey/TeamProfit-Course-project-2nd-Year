using Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClientPart.Dto.Cost
{
    public class CreateCostDto
    {
        [Required]
        public decimal Amount { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public CostType Type { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
