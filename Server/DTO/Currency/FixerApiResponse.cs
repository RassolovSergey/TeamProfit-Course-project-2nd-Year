using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Server.DTO.Currency
{
    /// <summary>
    /// Модель для десериализации ответа Fixer.io API
    /// </summary>
    public class FixerApiResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("base")]
        public string Base { get; set; } = null!;

        [JsonPropertyName("date")]
        public string Date { get; set; } = null!;

        [JsonPropertyName("rates")]
        public Dictionary<string, decimal>? Rates { get; set; }
    }
}
