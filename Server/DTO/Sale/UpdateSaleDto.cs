// Server/DTO/Sale/UpdateSaleDto.cs
using System.ComponentModel.DataAnnotations;

namespace Server.DTO.Sale
{
    /// <summary>DTO для обновления продажи/вклада</summary>
    public class UpdateSaleDto
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
