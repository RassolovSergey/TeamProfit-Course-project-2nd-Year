using Microsoft.AspNetCore.Authorization;   // Импортирует пространство имен Microsoft.AspNetCore.Authorization для атрибутов авторизации, таких как [AllowAnonymous].
using Microsoft.AspNetCore.Mvc;             // Импортирует пространство имен Microsoft.AspNetCore.Mvc, необходимое для создания API контроллеров и работы с HTTP-ответами.
using Server.DTO.User;                      // Импортирует пространство имен Server.DTO.User, содержащее Data Transfer Objects (DTO) для сущности пользователя.
using Server.Services.Interfaces;           // Импортирует пространство имен Server.Services.Interfaces, содержащее интерфейсы для сервисов, например IUserService.

// Определяет пространство имен для контроллеров сервера.
namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;         // Приватное поле только для чтения для хранения экземпляра сервиса пользователя.
        private readonly ILogger<AuthController> _logger;   // Приватное поле только для чтения для хранения экземпляра логгера. (Для записи логов о событиях)

        // Конструктор контроллера AuthController.
        // Использует механизм внедрения зависимостей (Dependency Injection) для получения экземпляров IUserService и ILogger<AuthController>.
        public AuthController(IUserService userService, ILogger<AuthController> logger) 
        {
            _userService = userService; // Присваивает полученный экземпляр сервиса пользователя приватному полю.
            _logger = logger;           // Присваивает полученный экземпляр логгера приватному полю.
        }

        [HttpPost("register")]
        [AllowAnonymous]    // Атрибут [AllowAnonymous] разрешает доступ к этому методу без аутентификации.
        // Объявление асинхронного метода Register.
        // Он возвращает Task<ActionResult<UserDto>>, что означает асинхронную операцию,
        // результатом которой будет ActionResult, содержащий UserDto (данные созданного пользователя) или другой HTTP-статус.
        // [FromBody] указывает, что параметр CreateUserDto (данные для создания пользователя) будет взят из тела HTTP-запроса.
        public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto dto)
        {
            // Блок try-catch для обработки возможных исключений во время выполнения метода.
            try
            {
                _logger.LogInformation("Register attempt for {Login}", dto.Login);  // Логирование информации о попытке регистрации.
                var createdUser = await _userService.CreateAsync(dto);              // Асинхронный вызов метода CreateAsync из _userService для создания нового пользователя.
                _logger.LogInformation("User {Login} registered with Id {Id}", dto.Login, createdUser.Id);  // Логирование информации об успешной регистрации пользователя с его логином и ID.
                // Возвращает HTTP-статус 201 Created.
                // CreatedAtAction генерирует URL для получения созданного ресурса (пользователя).
                // nameof(Register) указывает на текущий метод, new { id = createdUser.Id } предоставляет параметры маршрута для URL,
                // а createdUser – это тело ответа (данные созданного пользователя).
                return CreatedAtAction(nameof(Register), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                // Логирование ошибки, включая детали исключения (ex) и логин пользователя, при регистрации которого произошла ошибка.
                _logger.LogError(ex, "Error during registration for {Login}", dto.Login);
                // Возвращает HTTP-статус 500 Internal Server Error.
                // В теле ответа передается анонимный объект с сообщением об ошибке.
                return StatusCode(500, new { message = "Server error during registration." });
            }
        }
        [HttpPost("login")]
        [AllowAnonymous]    // Атрибут [AllowAnonymous] разрешает доступ к этому методу без аутентификации.
        // Объявление асинхронного метода Login.
        // Он возвращает Task<ActionResult<object>>, что означает асинхронную операцию,
        // результатом которой будет ActionResult, содержащий объект (в данном случае токен) или другой HTTP-статус.
        // [FromBody] указывает, что параметр LoginDto (данные для входа пользователя) будет взят из тела HTTP-запроса.
        public async Task<ActionResult<object>> Login([FromBody] LoginDto dto)
        {
            // Блок try-catch для обработки возможных исключений во время выполнения метода.
            try
            {
                _logger.LogInformation("Login attempt for {Login}", dto.Login); // Логирование информации о попытке входа в систему.
                // Асинхронный вызов метода AuthenticateAsync из _userService для аутентификации пользователя.
                // Передаются логин и пароль из DTO.
                var token = await _userService.AuthenticateAsync(dto.Login, dto.Password);

                // Проверка, был ли успешно сгенерирован токен.
                // Если токен равен null, это означает, что аутентификация не удалась (например, неверный логин или пароль).
                if (token == null)
                {
                    _logger.LogWarning("Invalid login or password for {Login}", dto.Login); // Логирование предупреждения о неверных учетных данных.
                    return Unauthorized(new { message = "Invalid login or password." });    // Возвращает HTTP-статус 401 Unauthorized с сообщением об ошибке.
                }

                _logger.LogInformation("User {Login} authenticated successfully", dto.Login);   // Логирование информации об успешной аутентификации пользователя.
                // Возвращает HTTP-статус 200 OK с токеном в теле ответа.
                return Ok(new { token, bearer = "Bearer " + token });

            }
            catch (Exception ex)
            {
                // Логирование ошибки, включая детали исключения (ex) и логин пользователя, при аутентификации которого произошла ошибка.
                _logger.LogError(ex, "Error during login for {Login}", dto.Login);
                // Возвращает HTTP-статус 500 Internal Server Error.
                // В теле ответа передается анонимный объект с сообщением об ошибке.
                return StatusCode(500, new { message = "Server error during authentication." });
            }
        }
    }
}