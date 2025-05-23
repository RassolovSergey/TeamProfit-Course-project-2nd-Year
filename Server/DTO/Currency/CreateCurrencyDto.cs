using System.ComponentModel.DataAnnotations;

namespace Server.DTO.Currency
{
    /// <summary>
    /// DTO для создания новой валюты
    /// </summary>
    public class CreateCurrencyDto
    {
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(5)]
        public string? Symbol { get; set; }
    }
}
