using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.DTO.Cost;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // все операции требуют валидного JWT
    public class CostsController : ControllerBase
    {
        private readonly ICostService _costService;
        private readonly IMapper _mapper;

        public CostsController(ICostService costService, IMapper mapper)
        {
            _costService = costService;
            _mapper = mapper;
        }

        /// <summary>GET /api/projects/{projectId}/costs — получить все траты проекта</summary>
        [HttpGet("/api/projects/{projectId}/costs")]
        [Authorize(Policy = "ProjectMember")]  // любой участник проекта
        public async Task<ActionResult<List<CostDto>>> GetByProject(int projectId)
        {
            var list = await _costService.GetAllAsync();
            return Ok(list.Where(c => c.ProjectId == projectId));
        }

        /// <summary>GET /api/costs/{id} — получить трату по id</summary>
        [HttpGet("{id:int}")]
        [Authorize(Policy = "ProjectMember")]  // любой участник проекта
        public async Task<ActionResult<CostDto>> GetById(int id)
        {
            var cost = await _costService.GetByIdAsync(id);
            return cost is null ? NotFound() : Ok(cost);
        }

        /// <summary>POST /api/projects/{projectId}/costs — создать трату</summary>
        [HttpPost("/api/projects/{projectId}/costs")]
        [Authorize(Policy = "ProjectAdmin")]   // только администратор проекта
        public async Task<ActionResult<CostDto>> Create(int projectId, CreateCostDto dto)
        {
            dto.ProjectId = projectId;
            var created = await _costService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>PUT /api/costs/{id} — обновить трату</summary>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "ProjectAdmin")]   // только администратор проекта
        public async Task<IActionResult> Update(int id, UpdateCostDto dto)
        {
            var updated = await _costService.UpdateAsync(id, dto);
            return updated is null ? NotFound() : NoContent();
        }

        /// <summary>DELETE /api/costs/{id} — удалить трату</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "ProjectAdmin")]   // только администратор проекта
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _costService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
