using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Data.Enums;

namespace Data.Entities
{
    /// <summary>
    /// Краудфандинговый проект
    /// </summary>
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>Дата старта проекта (задаётся пользователем)</summary>
        [Required]
        public DateTime DateStart { get; set; }

        /// <summary>Дата завершения = DateStart + ProjectDuration</summary>
        [Required]
        public DateTime DateClose { get; set; }

        /// <summary>Длительность проекта в днях</summary>
        [Required]
        public int ProjectDuration { get; set; }

        /// <summary>Текущий статус проекта</summary>
        public ProjectStatus? Status { get; set; }

        /// <summary>
        /// Коллекция наград для проекта
        /// </summary>
        public ICollection<Reward> Rewards { get; set; } = new List<Reward>();

        /// <summary>
        /// Коллекция расходов проекта
        /// </summary>
        public ICollection<Cost> Costs { get; set; } = new List<Cost>();

        /// <summary>
        /// Участники проекта с их условиями, в том числе админ (IsAdmin=true)
        /// </summary>
        public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();

        /// <summary>Валюта проекта</summary>
        [Required]
        public int CurrencyId { get; set; }

        public Currency Currency { get; set; } = null!;
    }
}
