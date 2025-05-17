namespace Server.DTO.Project
{
    /// <summary>Модель для обновления существующего проекта</summary>
    public class UpdateProjectDto
    {
        /// <summary>Новое название проекта</summary>
        public string Name { get; set; } = null!;

        /// <summary>Новое описание проекта</summary>
        public string? Description { get; set; }

        /// <summary>Новая дата старта (можно перенести запуск)</summary>
        public DateTime DateStart { get; set; }

        /// <summary>Новая длительность проекта в днях</summary>
        public int ProjectDuration { get; set; }
    }
}
