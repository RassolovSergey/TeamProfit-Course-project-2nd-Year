using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTO.User;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // все методы требуют валидного JWT
    public class UserController : ControllerBase
    {
        private readonly IUserService _svc;
        public UserController(IUserService svc) => _svc = svc;

        /// <summary>
        /// Получить пользователя по идентификатору
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var dto = await _svc.GetByIdAsync(id);
            return dto is null
                ? NotFound()
                : Ok(dto);
        }

        /// <summary>
        /// Обновить свои данные пользователя
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto dto)
        {
            // здесь можно проверить, что id из URL совпадает с id в JWT (чтобы пользователь
            // не смог подменить чужой профиль)
            var claimId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (claimId != id)
                return Forbid();

            try
            {
                var updated = await _svc.UpdateAsync(id, dto);
                return updated is null
                    ? NotFound()
                    : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Удалить свою учётную запись
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(claim, out var currentUserId) || currentUserId != id)
                return Forbid();

            var ok = await _svc.DeleteAsync(id);
            return ok
                ? NoContent()
                : NotFound();
        }
    }
}
