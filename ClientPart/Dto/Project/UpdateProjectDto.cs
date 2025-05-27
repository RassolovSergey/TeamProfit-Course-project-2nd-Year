using System.ComponentModel.DataAnnotations;

namespace Server.DTO.Project
{
    /// <summary>Модель для обновления существующего проекта</summary>
    public class UpdateProjectDto
    {
        [Required]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime DateStart { get; set; }

        [Required]
        public int ProjectDuration { get; set; }

        [Required]
        public int CurrencyId { get; set; }
    }

}
