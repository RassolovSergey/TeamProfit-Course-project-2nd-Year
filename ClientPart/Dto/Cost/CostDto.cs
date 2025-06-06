using Data.Enums;

namespace ClientPart.Dto.Cost
{
    public class CostDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public CostType Type { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }
}
