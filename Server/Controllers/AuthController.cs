using Microsoft.AspNetCore.Mvc;
using Server.DTO.User;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// POST /api/auth/register
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto dto)
        {
            var created = await _userService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(Register),
                new { id = created.Id },
                created);
        }

        /// <summary>
        /// Аутентификация и получение JWT
        /// POST /api/auth/login
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginDto dto)
        {
            var token = await _userService.AuthenticateAsync(dto.Login, dto.Password);
            if (token == null)
                return Unauthorized(new { message = "Invalid login or password." });

            // Возвращаем токен в простом объекте
            return Ok(new { token });
        }
    }
}
