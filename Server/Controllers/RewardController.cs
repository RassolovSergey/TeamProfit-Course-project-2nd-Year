// Server/Controllers/RewardsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.DTO.Reward;
using Server.DTO.Product;
using Server.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // все методы требуют валидного JWT
    public class RewardsController : ControllerBase
    {
        private readonly IRewardService _service;

        public RewardsController(IRewardService service)
        {
            _service = service;
        }

        /// <summary>GET api/rewards — все награды</summary>
        [HttpGet]
        public async Task<ActionResult<List<RewardDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        /// <summary>GET api/projects/{projectId}/rewards — все награды проекта</summary>
        [HttpGet("/api/projects/{projectId}/rewards")]
        [Authorize(Policy = "ProjectMember")] // любой участник проекта
        public async Task<ActionResult<List<RewardDto>>> GetByProject(int projectId)
            => Ok(await _service.GetByProjectAsync(projectId));

        /// <summary>GET api/rewards/{id} — детали награды</summary>
        [HttpGet("{id:int}")]
        [Authorize(Policy = "ProjectMember")] // любой участник проекта
        public async Task<ActionResult<RewardDto>> Get(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>POST api/projects/{projectId}/rewards — создать награду</summary>
        [HttpPost("/api/projects/{projectId}/rewards")]
        [Authorize(Policy = "ProjectAdmin")] // только администратор проекта
        public async Task<ActionResult<RewardDto>> Create(
            int projectId,
            [FromBody] CreateRewardDto dto)
        {
            var created = await _service.CreateAsync(dto, projectId);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>PUT api/rewards/{id} — обновить награду</summary>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "ProjectAdmin")] // только администратор проекта
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateRewardDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>DELETE api/rewards/{id} — удалить награду</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "ProjectAdmin")] // только администратор проекта
        public async Task<IActionResult> Delete(int id)
            => (await _service.DeleteAsync(id)) ? NoContent() : NotFound();


        /// <summary>POST api/rewards/{rewardId}/products/{productId} — привязать продукт</summary>
        [HttpPost("{rewardId:int}/products/{productId:int}")]
        [Authorize(Policy = "ProjectAdmin")] // только администратор проекта
        public async Task<IActionResult> AddProduct(int rewardId, int productId)
        {
            var ok = await _service.AddProductAsync(rewardId, productId);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>DELETE api/rewards/{rewardId}/products/{productId} — открепить продукт</summary>
        [HttpDelete("{rewardId:int}/products/{productId:int}")]
        [Authorize(Policy = "ProjectAdmin")] // только администратор проекта
        public async Task<IActionResult> RemoveProduct(
            int rewardId,
            int productId)
        {
            var ok = await _service.RemoveProductAsync(rewardId, productId);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>GET api/rewards/{rewardId}/products — все продукты награды</summary>
        [HttpGet("{rewardId:int}/products")]
        [Authorize(Policy = "ProjectMember")] // любой участник проекта
        public async Task<ActionResult<List<ProductDto>>> GetProducts(int rewardId)
        {
            var list = await _service.GetProductsByRewardAsync(rewardId);
            return Ok(list);
        }
    }
}
