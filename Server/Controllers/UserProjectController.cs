using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTO.UserProject;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserProjectsController : ControllerBase
    {
        private readonly IUserProjectService _service;
        public UserProjectsController(IUserProjectService service) =>
            _service = service;

        /// <summary>GET api/UserProjects/{projectId} — список участников проекта</summary>
        [HttpGet("{projectId:int}")]
        [Authorize(Policy = "ProjectMember")]
        public async Task<IActionResult> GetByProject(int projectId)
            => Ok(await _service.GetByProjectAsync(projectId));

        /// <summary>POST api/UserProjects/{projectId} — добавить участника в проект</summary>
        [HttpPost("{projectId:int}")]
        [Authorize(Policy = "ProjectAdmin")]
        public async Task<IActionResult> Create(
            int projectId,
            [FromBody] CreateUserProjectDto dto)
        {
            var created = await _service.CreateAsync(projectId, dto);

            return CreatedAtAction(
                nameof(GetByProject),
                new { projectId = created.ProjectId },
                created);
        }

        [HttpGet("mine")]
        public async Task<IActionResult> GetMine()
        {
            var rawUserId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(rawUserId, out var userId))
                return Unauthorized();

            var userProjects = await _service.GetByUserAsync(userId);
            return Ok(userProjects);
        }


        [HttpPut("{userId:int}/{projectId:int}")]
        [Authorize(Policy = "ProjectAdmin")]
        public async Task<IActionResult> Update(
            int userId,
            int projectId,
            [FromBody] UpdateUserProjectDto dto)
        {
            var updated = await _service.UpdateAsync(userId, projectId, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{userId:int}/{projectId:int}")]
        [Authorize(Policy = "ProjectAdmin")]
        public async Task<IActionResult> Delete(int userId, int projectId)
        {
            var ok = await _service.DeleteAsync(userId, projectId);
            return ok ? NoContent() : NotFound();
        }
    }
}

