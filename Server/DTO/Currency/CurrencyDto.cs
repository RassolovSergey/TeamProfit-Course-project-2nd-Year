namespace Server.DTO.Currency
{
    namespace Server.DTO.Currency
    {
        /// <summary>
        /// DTO для передачи данных о валюте клиенту
        /// </summary>
        public class CurrencyDto
        {
            /// <summary>Идентификатор валюты</summary>
            public int Id { get; set; }

            /// <summary>Код валюты (ISO 4217, 3 буквы)</summary>
            public string Code { get; set; }

            /// <summary>Полное название валюты</summary>
            public string Name { get; set; }

            /// <summary>Символ валюты (необязательно)</summary>
            public string? Symbol { get; set; }

            /// <summary>Текущий курс относительно базовой валюты</summary>
            public decimal RateToBase { get; set; }
        }
    }
}
