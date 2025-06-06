using Data.Enums;

namespace ClientPart.Dto.UserProject
{
    /// <summary>
    /// Модель для добавления участника в проект
    /// </summary>
    public class CreateUserProjectDto
    {
        /// <summary>Идентификатор пользователя</summary>
        public int UserId { get; set; }

        /// <summary>Тип сотрудничества</summary>
        public TypeCooperation TypeCooperation { get; set; }

        /// <summary>Фиксированная сумма вознаграждения</summary>
        public decimal FixedPrice { get; set; }

        /// <summary>Процент от прибыли</summary>
        public decimal PercentPrice { get; set; }
    }
}
