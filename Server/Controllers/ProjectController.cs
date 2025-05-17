using Microsoft.AspNetCore.Mvc;
using Server.DTO.Project;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsController(IProjectService service)
        {
            _service = service;
        }

        /// <summary>GET api/Projects</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        /// <summary>GET api/Projects/{id}</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>POST api/Projects</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        /// <summary>PUT api/Projects/{id}</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>DELETE api/Projects/{id}</summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
