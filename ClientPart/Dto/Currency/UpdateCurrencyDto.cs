using System.ComponentModel.DataAnnotations;

namespace ClientPart.Dto.Currency
{
    /// <summary>
    /// DTO для обновления существующей валюты
    /// </summary>
    public class UpdateCurrencyDto
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
