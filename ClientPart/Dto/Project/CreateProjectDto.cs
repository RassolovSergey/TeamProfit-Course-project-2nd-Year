using System.ComponentModel.DataAnnotations;
using Data.Enums;

namespace ClientPart.Dto.Project
{
    /// <summary>Модель для создания нового проекта</summary>
    public class CreateProjectDto
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
