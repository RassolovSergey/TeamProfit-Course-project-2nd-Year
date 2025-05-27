using Data.Enums;

namespace Server.DTO.UserProject
{
    /// <summary>Участник проекта с условием сотрудничества</summary>
    public class UserProjectDto
    {
        /// <summary>Id проекта</summary>
        public int ProjectId { get; set; }

        /// <summary>Id пользователя</summary>
        public int UserId { get; set; }

        /// <summary>Тип сотрудничества</summary>
        public TypeCooperation TypeCooperation { get; set; }

        /// <summary>Фиксированная сумма</summary>
        public decimal FixedPrice { get; set; }

        /// <summary>Процент от прибыли</summary>
        public decimal PercentPrice { get; set; }

        /// <summary>Сделать админом проекта</summary>
        public bool IsAdmin { get; set; }
    }
}
