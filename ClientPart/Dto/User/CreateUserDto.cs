﻿using System.ComponentModel.DataAnnotations;
using Data.Enums;

namespace ClientPart.Dto.User
{
    public class CreateUserDto
    {
        [Required, MaxLength(20)]
        public string Login { get; set; } = null!;

        [Required, EmailAddress, MaxLength(40)]
        public string Email { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;

    }
}
