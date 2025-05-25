// Server/Controllers/SalesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.DTO.Sale;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // все методы требуют валидного JWT
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _service;

        public SalesController(ISaleService service)
        {
            _service = service;
        }

        /// <summary>GET api/Sales — получить все продажи</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        /// <summary>GET api/projects/{projectId}/sales — получить все продажи по проекту</summary>
        [HttpGet("/api/projects/{projectId}/sales")]
        [Authorize(Policy = "ProjectMember")] // только участник проекта
        public async Task<IActionResult> GetByProject(int projectId)
        {
            var list = await _service.GetByProjectAsync(projectId);
            return Ok(list);
        }

        /// <summary>GET api/Sales/{id} — получить продажу по id</summary>
        [HttpGet("{id:int}")]
        [Authorize(Policy = "ProjectMember")] // только участник проекта
        public async Task<IActionResult> Get(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>POST api/rewards/{rewardId}/sales — создать продажу для награды</summary>
        [HttpPost("/api/rewards/{rewardId}/sales")]
        [Authorize(Policy = "ProjectMember")] // только участник проекта
        public async Task<IActionResult> Create(
            int rewardId,
            [FromBody] CreateSaleDto dto)
        {
            var created = await _service.CreateAsync(dto, rewardId);
            return CreatedAtAction(
                nameof(Get),
                new { id = created.Id },
                created);
        }

        /// <summary>PUT api/Sales/{id} — обновить продажу</summary>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "ProjectAdmin")] // только администратор проекта
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateSaleDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>DELETE api/Sales/{id} — удалить продажу</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "ProjectAdmin")] // только администратор проекта
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
