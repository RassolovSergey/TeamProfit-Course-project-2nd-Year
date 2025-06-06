using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTO.Product;
using Server.DTO.Reward;
using Server.Services.Implementations;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId:int}/rewards")]
    [Authorize]
    public class RewardController : ControllerBase
    {
        private readonly IRewardService _rewardService;
        private readonly IUserProjectService _userProjectService;

        public RewardController(IRewardService rewardService, IUserProjectService userProjectService)
        {
            _rewardService = rewardService;
            _userProjectService = userProjectService;
        }

        // 1) Получить все награды проекта (участник или админ)
        [HttpGet]
        public async Task<IActionResult> GetAll(int projectId)
        {
            var userId = GetUserId();
            try
            {
                var rewards = await _rewardService.GetByProjectAsync(projectId, userId);
                return Ok(rewards);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid(); // Возвращает 403 Forbidden
            }
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
        [HttpPost("/api/rewards/{rewardId:int}/products")]
        public async Task<IActionResult> AddProduct(int rewardId, [FromBody] CreateProductDto dto)
        {
            var userId = GetUserId();

            try
            {
                var product = await _rewardService.AddProductToRewardAsync(rewardId, dto, userId);
                var rewardEntity = await _rewardService.GetEntityByIdAsync(rewardId);
                return CreatedAtAction(nameof(GetProducts), new { projectId = rewardEntity.ProjectId, rewardId }, product);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Reward not found" });
            }
        }



        // 7) Обновить продукт по Id (только админ)
        [HttpPut("/api/products/{productId:int}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] UpdateProductDto dto)
        {
            var userId = GetUserId(); // ваш метод извлечения ID пользователя из JWT

            try
            {
                var updated = await _rewardService.UpdateProductAsync(productId, dto, userId);
                if (updated == null)
                    return NotFound();    // продукт не найден

                return Ok(updated);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();           // не админ — 403
            }
            catch (KeyNotFoundException)
            {
                return NotFound();         // на всякий случай, если в сервисе бросают KeyNotFound
            }
        }


        // 8) Удалить продукт по Id (только админ)
        [HttpDelete("/api/products/{productId:int}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var userId = GetUserId(); // метод получения userId из JWT

            try
            {
                var success = await _rewardService.DeleteProductByIdAsync(productId, userId);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }


        private int GetUserId()
        {
            var raw = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(raw, out var userId) ? userId : throw new UnauthorizedAccessException();
        }
    }
}
