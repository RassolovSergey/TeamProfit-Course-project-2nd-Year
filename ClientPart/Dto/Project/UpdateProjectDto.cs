// Server/DTO/Project/UpdateProjectDto.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace ClientPart.Dto.Project
{
    public class UpdateProjectDto
    {
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>Дата начала проекта</summary>
        public DateTime? DateStart { get; set; }

        /// <summary>Длительность в днях</summary>
        public int? ProjectDuration { get; set; }

        /// <summary>Id валюты</summary>
        public int? CurrencyId { get; set; }
    }
}
