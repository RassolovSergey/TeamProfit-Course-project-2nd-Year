using System.ComponentModel.DataAnnotations;

namespace ClientPart.Dto.User
{
    public class LoginDto
    {
        [Required, MaxLength(20)]
        public string Login { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;
    }
}
