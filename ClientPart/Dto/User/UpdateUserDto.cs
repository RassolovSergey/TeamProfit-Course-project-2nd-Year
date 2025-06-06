// Server/DTO/User/UpdateUserDto.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClientPart.Dto.User
{
    public class UpdateUserDto : IValidatableObject
    {
        [MaxLength(20)]
        public string? Login { get; set; }

        [EmailAddress, MaxLength(40)]
        public string? Email { get; set; }

        [MaxLength(40)]
        public string? Name { get; set; }

        [MaxLength(40)]
        public string? SurName { get; set; }

        /// <summary>Id валюты, выбранной пользователем</summary>
        public int? CurrencyId { get; set; }

        /// <summary>Текущий пароль (для подтверждения смены)</summary>
        [MinLength(6)]
        public string? CurrentPassword { get; set; }

        /// <summary>Новый пароль (минимум 6 символов)</summary>
        [MinLength(6)]
        public string? NewPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
        {
            // Если один из полей пароля задан, обязаны оба
            if (!string.IsNullOrEmpty(CurrentPassword) || !string.IsNullOrEmpty(NewPassword))
            {
                if (string.IsNullOrEmpty(CurrentPassword))
                    yield return new ValidationResult(
                        "Требуется указать текущий пароль при смене.",
                        new[] { nameof(CurrentPassword) });

                if (string.IsNullOrEmpty(NewPassword))
                    yield return new ValidationResult(
                        "Требуется указать новый пароль.",
                        new[] { nameof(NewPassword) });
            }
        }
    }
}
