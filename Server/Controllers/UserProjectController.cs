using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.DTO.UserProject;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProjectController : ControllerBase
    {
        private readonly IUserProjectService _svc;

        public UserProjectController(IUserProjectService svc)
        {
            _svc = svc;
        }

        /// <summary>
        /// GET /api/UserProject
        /// Возвращает все записи UserProject
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<UserProjectDto>>> GetAll()
            => Ok(await _svc.GetAllAsync());

        /// <summary>
        /// GET /api/UserProject/{userId}/{projectId}
        /// Возвращает одну запись по составному ключу
        /// </summary>
        [HttpGet("{userId:int}/{projectId:int}")]
        public async Task<ActionResult<UserProjectDto>> Get(int userId, int projectId)
        {
            var dto = await _svc.GetAsync(userId, projectId);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// POST /api/UserProject
        /// Создает новую запись UserProject
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserProjectDto>> Create([FromBody] CreateUserProjectDto dto)
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get),
                new { userId = created.UserId, projectId = created.ProjectId },
                created);
        }

        /// <summary>
        /// PUT /api/UserProject/{userId}/{projectId}
        /// Обновляет запись по составному ключу
        /// </summary>
        [HttpPut("{userId:int}/{projectId:int}")]
        public async Task<ActionResult<UserProjectDto>> Update(
            int userId,
            int projectId,
            [FromBody] UpdateUserProjectDto dto)
        {
            var updated = await _svc.UpdateAsync(userId, projectId, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>
        /// DELETE /api/UserProject/{userId}/{projectId}
        /// Удаляет запись по составному ключу
        /// </summary>
        [HttpDelete("{userId:int}/{projectId:int}")]
        public async Task<IActionResult> Delete(int userId, int projectId)
            => await _svc.DeleteAsync(userId, projectId)
                ? NoContent()
                : NotFound();
    }
}
