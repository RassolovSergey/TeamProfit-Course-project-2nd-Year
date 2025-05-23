// Server/Controllers/RewardsController.cs
using Microsoft.AspNetCore.Mvc;
using Server.DTO.Reward;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RewardsController : ControllerBase
    {
        private readonly IRewardService _service;

        public RewardsController(IRewardService service)
        {
            _service = service;
        }

        /// <summary>Получить все награды</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        /// <summary>Получить все награды по проекту</summary>
        [HttpGet("/api/projects/{projectId}/rewards")]
        public async Task<IActionResult> GetByProject(int projectId)
        {
            var list = await _service.GetByProjectAsync(projectId);
            return Ok(list);
        }

        /// <summary>Получить награду по id</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>Создать награду для проекта</summary>
        [HttpPost("/api/projects/{projectId}/rewards")]
        public async Task<IActionResult> Create(int projectId, [FromBody] CreateRewardDto dto)
        {
            dto.ProjectId = projectId;
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>Обновить награду</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRewardDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>Удалить награду</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
