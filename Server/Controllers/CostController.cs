// — Принимает HTTP-запросы (GET/POST/PUT/DELETE), извлекает/валидирует входящие DTO.
// — Вызывает соответствующий метод сервиса (ITeamService, IUserService и т. д.).
// — Возвращает IActionResult с нужным HTTP-кодом и ответным DTO.

using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostController : ControllerBase
    {
    }
}
