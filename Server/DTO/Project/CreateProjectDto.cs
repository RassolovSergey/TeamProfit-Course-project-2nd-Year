using Data.Enums;

namespace Server.DTO.Project
{
    /// <summary>Модель для создания нового проекта</summary>
    public class CreateProjectDto
    {
        /// <summary>Название проекта</summary>
        public string Name { get; set; } = null!;

        /// <summary>Описание проекта</summary>
        public string? Description { get; set; }

        /// <summary>Дата старта (задаётся пользователем)</summary>
        public DateTime DateStart { get; set; }

        /// <summary>Длительность проекта в днях</summary>
        public int ProjectDuration { get; set; }

        /// <summary>Первоначальный статус проекта</summary>
        public ProjectStatus Status { get; set; }

        /// <summary>Идентификатор пользователя, создающего проект (будет админом)</summary>
        public int CreatorUserId { get; set; }
    }
}
