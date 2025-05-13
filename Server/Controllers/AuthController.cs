using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.DTO.User;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService,
                              ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto dto)
        {
            try
            {
                _logger.LogInformation("Register attempt for {Login}", dto.Login);
                var created = await _userService.CreateAsync(dto);
                _logger.LogInformation("User {Login} registered with Id {Id}", dto.Login, created.Id);
                return CreatedAtAction(nameof(Register), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for {Login}", dto.Login);
                return StatusCode(500, new { message = "Server error during registration." });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginDto dto)
        {
            try
            {
                _logger.LogInformation("Login attempt for {Login}", dto.Login);
                var token = await _userService.AuthenticateAsync(dto.Login, dto.Password);

                if (token == null)
                {
                    _logger.LogWarning("Invalid login or password for {Login}", dto.Login);
                    return Unauthorized(new { message = "Invalid login or password." });
                }

                _logger.LogInformation("User {Login} authenticated successfully", dto.Login);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for {Login}", dto.Login);
                return StatusCode(500, new { message = "Server error during authentication." });
            }
        }
    }
}
