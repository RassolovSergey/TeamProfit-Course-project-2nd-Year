// Server/Controllers/UserController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.DTO.User;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]  // все операции, кроме регистрации, требуют валидного JWT
    public class UserController : ControllerBase
    {
        private readonly IUserService _svc;

        public UserController(IUserService svc) => _svc = svc;

        /// <summary>
        /// Получить список всех пользователей
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
            => Ok(await _svc.GetAllAsync());

        /// <summary>
        /// Получить пользователя по идентификатору
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var dto = await _svc.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Создать нового пользователя (регистрация)  
        /// Открыто для всех
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>
        /// Обновить данные пользователя  
        /// Доступно авторизованному пользователю
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto dto)
        {
            var updated = await _svc.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>
        /// Удалить пользователя  
        /// Доступно авторизованному пользователю
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _svc.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
