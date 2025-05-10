using Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Server.DTO.User
{
    public class UpdateUserDto
    {
        [Required, MaxLength(20)]
        public string Login { get; set; } = null!;

        [Required, EmailAddress, MaxLength(40)]
        public string Email { get; set; } = null!;

        [Required]
        public TypeCooperation TypeCooperation { get; set; }

        public decimal? FixedPrice { get; set; }
        public byte? PercentPrice { get; set; }
    }
}
