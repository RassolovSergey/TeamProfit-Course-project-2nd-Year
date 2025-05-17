using Data.Enums;
using Server.DTO.UserProject;

namespace Server.DTO.Project
{
    /// <summary>Проект, возвращаемый клиенту</summary>
    public class ProjectDto
    {
        public int Id { get; set; }

        /// <summary>Название проекта</summary>
        public string Name { get; set; } = null!;

        /// <summary>Описание проекта (опционально)</summary>
        public string? Description { get; set; }

        /// <summary>Дата старта проекта</summary>
        public DateTime DateStart { get; set; }

        /// <summary>Дата завершения проекта (DateStart + ProjectDuration)</summary>
        public DateTime DateClose { get; set; }

        /// <summary>Длительность проекта в днях</summary>
        public int ProjectDuration { get; set; }

        /// <summary>Текущий статус проекта</summary>
        public ProjectStatus? Status { get; set; }

        /// <summary>Список участников с их ролями (включая администратора)</summary>
        public List<UserProjectDto> Participants { get; set; } = new();
    }
}
