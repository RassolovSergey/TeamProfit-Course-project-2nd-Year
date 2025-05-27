using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTO.Product;
using Server.DTO.Reward;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId:int}/rewards")]
    [Authorize]
    public class RewardController : ControllerBase
    {
        private readonly IRewardService _rewardService;

        public RewardController(IRewardService rewardService)
        {
            _rewardService = rewardService;
        }

        // 1) Получить все награды проекта (участник или админ)
        [HttpGet]
        public async Task<IActionResult> GetAll(int projectId)
        {
            var userId = GetUserId();
            var rewards = await _rewardService.GetByProjectAsync(projectId, userId);
            return Ok(rewards);
        }

        // 2) Добавить награду (только админ)
        [HttpPost]
        [Authorize(Policy = "ProjectAdmin")]
        public async Task<IActionResult> Create(int projectId, [FromBody] CreateRewardDto dto)
        {
            var userId = GetUserId();
            var reward = await _rewardService.CreateRewardAsync(projectId, dto, userId);
            return CreatedAtAction(nameof(GetById), new { projectId, rewardId = reward.Id }, reward);
        }

        // Получить награду по Id
        [HttpGet("{rewardId:int}")]
        [Authorize(Policy = "ProjectMember")]
        public async Task<IActionResult> GetById(int projectId, int rewardId)
        {
            var userId = GetUserId();
            var rewards = await _rewardService.GetByProjectAsync(projectId, userId);
            var reward = rewards.FirstOrDefault(r => r.Id == rewardId);
            if (reward == null) return NotFound();
            return Ok(reward);
        }

        // 3) Обновить награду (только админ)
        [HttpPut("{rewardId:int}")]
        [Authorize(Policy = "ProjectAdmin")]
        public async Task<IActionResult> Update(int rewardId, [FromBody] UpdateRewardDto dto)
        {
            var userId = GetUserId();
            var updated = await _rewardService.UpdateRewardAsync(rewardId, dto, userId);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // 4) Удалить награду (только админ)
        [HttpDelete("{rewardId:int}")]
        [Authorize(Policy = "ProjectAdmin")]
        public async Task<IActionResult> Delete(int rewardId)
        {
            var userId = GetUserId();
            var deleted = await _rewardService.DeleteRewardAsync(rewardId, userId);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // 5) Получить продукты награды (участник или админ)
        [HttpGet("{rewardId:int}/products")]
        public async Task<IActionResult> GetProducts(int rewardId)
        {
            var userId = GetUserId();
            var products = await _rewardService.GetProductsByRewardAsync(rewardId, userId);
            return Ok(products);
        }

        // 6) Добавить продукт к награде (только админ)
        [HttpPost("{rewardId:int}/products")]
        [Authorize(Policy = "ProjectAdmin")]
        public async Task<IActionResult> AddProduct(int rewardId, [FromBody] CreateProductDto dto)
        {
            var userId = GetUserId();
            var product = await _rewardService.AddProductToRewardAsync(rewardId, dto, userId);
            return CreatedAtAction(nameof(GetProducts), new { rewardId }, product);
        }

        // 7) Обновить продукт по Id (только админ)
        [HttpPut("{rewardId:int}/products/{productId:int}")]
        [Authorize(Policy = "ProjectAdmin")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] UpdateProductDto dto)
        {
            var userId = GetUserId();
            var updated = await _rewardService.UpdateProductAsync(productId, dto, userId);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // 8) Удалить продукт по Id (только админ)
        [HttpDelete("{rewardId:int}/products/{productId:int}")]
        [Authorize(Policy = "ProjectAdmin")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var userId = GetUserId();
            var deleted = await _rewardService.DeleteProductAsync(productId, userId);
            if (!deleted) return NotFound();
            return NoContent();
        }

        private int GetUserId()
        {
            var raw = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(raw, out var userId) ? userId : throw new UnauthorizedAccessException();
        }
    }
}
