// Server/Controllers/RewardsController.cs
using Microsoft.AspNetCore.Mvc;
using Server.DTO.Reward;
using Server.DTO.Product;
using Server.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<ActionResult<List<RewardDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("/api/projects/{projectId}/rewards")]
        public async Task<ActionResult<List<RewardDto>>> GetByProject(int projectId)
            => Ok(await _service.GetByProjectAsync(projectId));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RewardDto>> Get(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost("/api/projects/{projectId}/rewards")]
        public async Task<ActionResult<RewardDto>> Create(
            int projectId,
            [FromBody] CreateRewardDto dto)
        {
            var created = await _service.CreateAsync(dto, projectId);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateRewardDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => (await _service.DeleteAsync(id)) ? NoContent() : NotFound();


        // Привязать продукт к награде
        [HttpPost("{rewardId:int}/products/{productId:int}")]
        public async Task<IActionResult> AddProduct(
            int rewardId,
            int productId)
        {
            var ok = await _service.AddProductAsync(rewardId, productId);
            return ok ? NoContent() : NotFound();
        }

        // Открепить продукт от награды
        [HttpDelete("{rewardId:int}/products/{productId:int}")]
        public async Task<IActionResult> RemoveProduct(
            int rewardId,
            int productId)
        {
            var ok = await _service.RemoveProductAsync(rewardId, productId);
            return ok ? NoContent() : NotFound();
        }

        // Получить все продукты награды
        [HttpGet("{rewardId:int}/products")]
        public async Task<ActionResult<List<ProductDto>>> GetProducts(
            int rewardId)
        {
            var list = await _service.GetProductsByRewardAsync(rewardId);
            return Ok(list);
        }
    }
}
