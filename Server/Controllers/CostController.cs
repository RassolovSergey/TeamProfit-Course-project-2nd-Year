using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.DTO.Cost;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CostsController : ControllerBase
    {
        private readonly ICostService _costService;
        private readonly IMapper _mapper;

        public CostsController(ICostService costService, IMapper mapper)
        {
            _costService = costService;
            _mapper = mapper;
        }

        /// <summary>Получить все траты по проекту</summary>
        [HttpGet("/api/projects/{projectId}/costs")]
        public async Task<ActionResult<List<CostDto>>> GetByProject(int projectId)
        {
            var list = await _costService.GetAllAsync();
            return Ok(list.Where(c => c.ProjectId == projectId));
        }

        /// <summary>Получить трату по id</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CostDto>> GetById(int id)
        {
            var cost = await _costService.GetByIdAsync(id);
            if (cost == null)
                return NotFound();
            return Ok(cost);
        }

        /// <summary>Создать трату для проекта</summary>
        [HttpPost("/api/projects/{projectId}/costs")]
        public async Task<ActionResult<CostDto>> Create(int projectId, CreateCostDto dto)
        {
            dto.ProjectId = projectId;
            var created = await _costService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>Обновить трату по id</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCostDto dto)
        {
            var updated = await _costService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();
            return NoContent();
        }

        /// <summary>Удалить трату по id</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _costService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
