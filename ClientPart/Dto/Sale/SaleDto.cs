// Server/DTO/Sale/SaleDto.cs
namespace ClientPart.Dto.Sale
{
    /// <summary>DTO для возврата информации о продаже/вкладе</summary>
    public class SaleDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string BackerName { get; set; } = null!;
        public string BackerEmail { get; set; } = null!;
        public int RewardId { get; set; }
    }
}
