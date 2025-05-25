using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.DTO.Currency;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    /// <summary>
    /// Контроллер для управления справочником валют
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Все операции требуют валидного JWT
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyService _service;

        public CurrenciesController(ICurrencyService service)
        {
            _service = service;
        }

        /// <summary>GET api/Currencies — получить все валюты</summary>
        [HttpGet]
        [Authorize]  // любой авторизованный пользователь
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        /// <summary>GET api/Currencies/{id} — получить валюту по id</summary>
        [HttpGet("{id:int}")]
        [Authorize]  // любой авторизованный пользователь
        public async Task<IActionResult> Get(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>POST api/Currencies — создать валюту</summary>
        [HttpPost]
        [Authorize(Policy = "ProjectAdmin")] // только администратор проекта
        public async Task<IActionResult> Create([FromBody] CreateCurrencyDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>PUT api/Currencies/{id} — обновить валюту</summary>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "ProjectAdmin")] // только администратор проекта
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCurrencyDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>DELETE api/Currencies/{id} — удалить валюту</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "ProjectAdmin")] // только администратор проекта
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
