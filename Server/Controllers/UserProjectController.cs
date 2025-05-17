using Microsoft.AspNetCore.Mvc;
using Server.DTO.UserProject;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProjectsController : ControllerBase
    {
        private readonly IUserProjectService _service;
        public UserProjectsController(IUserProjectService service) => _service = service;

        [HttpGet("{projectId:int}")]
        public async Task<IActionResult> GetByProject(int projectId)
            => Ok(await _service.GetByProjectAsync(projectId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserProjectDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetByProject),
                new { projectId = created.ProjectId },
                created);
        }

        [HttpPut("{userId:int}/{projectId:int}")]
        public async Task<IActionResult> Update(int userId, int projectId, [FromBody] UpdateUserProjectDto dto)
        {
            var updated = await _service.UpdateAsync(userId, projectId, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{userId:int}/{projectId:int}")]
        public async Task<IActionResult> Delete(int userId, int projectId)
        {
            var ok = await _service.DeleteAsync(userId, projectId);
            return ok ? NoContent() : NotFound();
        }
    }
}
