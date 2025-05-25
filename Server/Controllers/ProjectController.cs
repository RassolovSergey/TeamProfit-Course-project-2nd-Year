using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.DTO.Project;
using Server.Services.Interfaces;
using AutoMapper;
using Data.Context;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public ProjectsController(
            IProjectService service,
            IMapper mapper,
            AppDbContext db)
        {
            _service = service;
            _mapper = mapper;
            _db = db;
        }

        /// <summary>GET api/Projects — список всех проектов</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        /// <summary>GET api/Projects/{id}</summary>
        [HttpGet("{id:int}")]
        [Authorize(Policy = "ProjectMember")]
        public async Task<IActionResult> Get(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>POST api/Projects — создать новый проект</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
        {
            var created = await _service.CreateAsync(dto);
            // Важно! Используем "id", т.к. это имя параметра в методе Get
            return CreatedAtAction(
                nameof(Get),
                new { id = created.Id },
                created);
        }

        /// <summary>PUT api/Projects/{id} — обновить проект</summary>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "ProjectAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        /// <summary>DELETE api/Projects/{id} — удалить проект</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "ProjectAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
