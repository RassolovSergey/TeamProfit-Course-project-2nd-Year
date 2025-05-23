using Microsoft.AspNetCore.Mvc;
using Server.DTO.Sale;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _service;

        public SalesController(ISaleService service)
        {
            _service = service;
        }

        /// <summary>Получить все продажи (все проекты)</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        /// <summary>Получить все продажи для проекта</summary>
        [HttpGet("/api/projects/{projectId}/sales")]
        public async Task<IActionResult> GetByProject(int projectId)
        {
            var list = await _service.GetByProjectAsync(projectId);
            return Ok(list);
        }

        /// <summary>Получить продажу по id</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>Создать продажу для награды</summary>
        [HttpPost("/api/rewards/{rewardId}/sales")]
        public async Task<IActionResult> Create(int rewardId, [FromBody] CreateSaleDto dto)
        {
            dto.RewardId = rewardId;
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>Обновить продажу</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSaleDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>Удалить продажу</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
