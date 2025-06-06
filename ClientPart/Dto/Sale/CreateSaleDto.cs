// Server/DTO/Sale/CreateSaleDto.cs
using System.ComponentModel.DataAnnotations;

namespace ClientPart.Dto.Sale
{
    /// <summary>DTO для создания продажи/вклада</summary>
    public class CreateSaleDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string BackerName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string BackerEmail { get; set; } = null!;
    }
}
