// Server/Controllers/UserProjectsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.DTO.UserProject;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // все методы требуют валидного JWT
    public class UserProjectsController : ControllerBase
    {
        private readonly IUserProjectService _service;

        public UserProjectsController(IUserProjectService service) =>
            _service = service;

        /// <summary>GET api/UserProjects/{projectId} — список участников проекта</summary>
        [HttpGet("{projectId:int}")]
        [Authorize(Policy = "ProjectMember")]  // любой участник проекта
        public async Task<IActionResult> GetByProject(int projectId)
            => Ok(await _service.GetByProjectAsync(projectId));

        /// <summary>POST api/UserProjects — добавить участника в проект</summary>
        [HttpPost]
        [Authorize(Policy = "ProjectAdmin")]   // только админ проекта
        public async Task<IActionResult> Create([FromBody] CreateUserProjectDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetByProject),
                new { projectId = created.ProjectId },
                created);
        }

        /// <summary>PUT api/UserProjects/{userId}/{projectId} — обновить участника проекта</summary>
        [HttpPut("{userId:int}/{projectId:int}")]
        [Authorize(Policy = "ProjectAdmin")]   // только админ проекта
        public async Task<IActionResult> Update(
            int userId,
            int projectId,
            [FromBody] UpdateUserProjectDto dto)
        {
            var updated = await _service.UpdateAsync(userId, projectId, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>DELETE api/UserProjects/{userId}/{projectId} — удалить участника проекта</summary>
        [HttpDelete("{userId:int}/{projectId:int}")]
        [Authorize(Policy = "ProjectAdmin")]   // только админ проекта
        public async Task<IActionResult> Delete(int userId, int projectId)
        {
            var ok = await _service.DeleteAsync(userId, projectId);
            return ok ? NoContent() : NotFound();
        }
    }
}
