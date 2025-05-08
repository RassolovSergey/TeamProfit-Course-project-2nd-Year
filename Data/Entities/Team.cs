using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Team
    {
        // Поля
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string Name { get; set; } = "Новая команда";


         
        // Связи

        // Администратор команды
        public int AdminId { get; set; }
        public User? Admin { get; set; }

        // Участники команды
        public ICollection<User> Users { get; set; } = new List<User>();

        // Связанный проект (1:1)
        public Project? Project { get; set; }
    }
}
