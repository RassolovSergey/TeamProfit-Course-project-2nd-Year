using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.DTO.Project;
using Server.Services.Interfaces;
using AutoMapper;
using Data.Context;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Server.DTO.User;

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

        // КОНТРОЛЕРЫ СПИСКОВ СУЩНОСТЕЙ

        // ПОЛУЧИТЬ ВСЕ ПРОЕКТЫ ПОЛЬЗОВАТЕЛЯ
        /// <summary>GET api/Projects — список всех проектов пользователя</summary>
        [HttpGet("mine")]
        public async Task<IActionResult> GetMine()
        {
            var raw = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                   ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(raw, out var userId))
                return Unauthorized();

            var list = await _service.GetByUserAsync(userId);
            return Ok(list);
        }



        // КОНТРОЛЕРЫ СУЩНОСТЕЙ

        // ПОЛУЧИТЬ ПРОЕКТ по Id
        /// <summary>GET api/Projects/{id}</summary>
        [HttpGet("{id:int}")]
        [Authorize(Policy = "ProjectMember")]
        public async Task<IActionResult> Get(int id)

        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        // СОЗДАТЬ ПРОЕКТ 
        /// <summary>POST api/Projects — создать новый проект</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
        {
            // вытаскиваем из JWT-claims id текущего пользователя
            var claim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                         ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(claim, out var userId))
                return Unauthorized();

            var created = await _service.CreateAsync(dto, userId);

            return CreatedAtAction(
                nameof(Get),
                new { id = created.Id },
                created
            );
        }


        // ОБНОВИТЬ ПРОЕКТ по Id
        /// <summary>PUT api/Projects/{id} — обновить проект</summary>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "ProjectAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }



        // УДАЛИТЬ ПРОЕКТ по Id
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
