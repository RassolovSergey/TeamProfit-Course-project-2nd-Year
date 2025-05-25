// Server/Controllers/ProductsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.DTO.Product;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // все методы требуют валидного JWT
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        /// <summary>GET api/Products — получить все продукты</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        /// <summary>GET api/Products/{id} — получить продукт по id</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>POST api/Products — создать продукт</summary>
        [HttpPost]
        [Authorize(Policy = "ProjectAdmin")] // только администратор может создавать продукты
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>PUT api/Products/{id} — обновить продукт</summary>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "ProjectAdmin")] // только администратор может обновлять продукты
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>DELETE api/Products/{id} — удалить продукт</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "ProjectAdmin")] // только администратор может удалять продукты
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>GET api/rewards/{rewardId}/products — получить продукты по награде</summary>
        [HttpGet("/api/rewards/{rewardId}/products")]
        [Authorize(Policy = "ProjectMember")] // любой участник соответствующего проекта
        public async Task<IActionResult> GetByReward(int rewardId)
        {
            var list = await _service.GetByRewardAsync(rewardId);
            return Ok(list);
        }
    }
}
