using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.DTO.User;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Контроллер для работы с пользователями
    public class UserController : ControllerBase
    {
        // Сервис для работы с пользователями (внедряется через DI)
        private readonly IUserService _svc;
        // Конструктор с внедрением зависимости IUserService
        public UserController(IUserService svc) => _svc = svc;

        /// <summary>
        /// Получить список всех пользователей
        /// </summary>
        /// <returns>Список пользователей в формате UserDto</returns>
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
            => Ok(await _svc.GetAllAsync());

        /// <summary>
        /// Получить пользователя по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns>
        /// 200 - пользователь найден (UserDto)
        /// 404 - пользователь не найден
        /// </returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var dto = await _svc.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        /// <param name="dto">Данные для создания пользователя</param>
        /// <returns>
        /// 201 - пользователь успешно создан (с Location в заголовках)
        /// 400 - неверные данные в запросе
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="dto">Новые данные пользователя</param>
        /// <returns>
        /// 200 - пользователь успешно обновлен (UserDto)
        /// 400 - неверные данные в запросе
        /// 404 - пользователь не найден
        /// </returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto dto)
        {
            var updated = await _svc.UpdateAsync(id, dto);
            if (updated is null)
                return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns>
        /// 204 - пользователь успешно удален
        /// 404 - пользователь не найден
        /// </returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _svc.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
