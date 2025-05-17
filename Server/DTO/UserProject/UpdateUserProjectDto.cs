using Data.Enums;

namespace Server.DTO.UserProject
{
    /// <summary>
    /// Модель для обновления условий сотрудничества участника
    /// </summary>
    public class UpdateUserProjectDto
    {
        /// <summary>Тип сотрудничества</summary>
        public TypeCooperation TypeCooperation { get; set; }

        /// <summary>Новая фиксированная сумма</summary>
        public decimal FixedPrice { get; set; }

        /// <summary>Новый процент от прибыли</summary>
        public decimal PercentPrice { get; set; }
    }
}
