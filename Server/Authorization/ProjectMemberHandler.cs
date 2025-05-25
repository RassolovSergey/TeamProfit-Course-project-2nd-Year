using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Server.Services.Interfaces;

namespace Server.Authorization
{
    /// <summary>Handler для проверки, что пользователь – участник проекта</summary>
    public class ProjectMemberHandler
    : AuthorizationHandler<ProjectMemberRequirement>
    {
        private readonly IUserProjectService _up;

        public ProjectMemberHandler(IUserProjectService up) => _up = up;

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ProjectMemberRequirement requirement)
        {
            // извлекаем HttpContext
            if (context.Resource is not AuthorizationFilterContext mvc)
                return;

            // ищем сначала projectId, потом id
            var rd = mvc.RouteData.Values;
            var key = rd.ContainsKey("projectId") ? "projectId"
                    : rd.ContainsKey("id") ? "id"
                    : null;
            if (key is null)
                return;

            if (!int.TryParse(rd[key]!.ToString(), out var projectId))
                return;

            // вытаскиваем userId из токена
            var userId = int.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // проверяем, что в UserProject есть запись
            if (await _up.AnyAsync(up => up.ProjectId == projectId && up.UserId == userId))
                context.Succeed(requirement);
        }
    }

}
