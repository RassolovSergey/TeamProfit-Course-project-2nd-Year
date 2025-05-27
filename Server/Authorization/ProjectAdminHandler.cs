// ProjectAdminHandler.cs
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Server.Services.Interfaces;

namespace Server.Authorization
{
    /// <summary>
    /// Handler для проверки, что пользователь – администратор проекта
    /// </summary>
    public class ProjectAdminHandler
        : AuthorizationHandler<ProjectAdminRequirement>
    {
        private readonly IUserProjectService _up;
        private readonly ILogger<ProjectAdminHandler> _logger;

        public ProjectAdminHandler(
            IUserProjectService up,
            ILogger<ProjectAdminHandler> logger)
        {
            _up = up;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ProjectAdminRequirement requirement)
        {
            _logger.LogInformation("ProjectAdminHandler invoked. ResourceType={ResourceType}",
                context.Resource?.GetType().Name ?? "<null>");

            int? projectId = null;

            // 1) Смотрим, что мы там получили: MVC-контекст или «сырый» HttpContext
            if (context.Resource is AuthorizationFilterContext mvcCtx)
            {
                if (mvcCtx.RouteData.Values.TryGetValue("projectId", out var raw1)
                    || mvcCtx.RouteData.Values.TryGetValue("id", out raw1))
                {
                    if (int.TryParse(raw1?.ToString(), out var pid1))
                        projectId = pid1;
                }
            }
            else if (context.Resource is HttpContext httpCtx)
            {
                var rd = httpCtx.GetRouteData();
                if (rd.Values.TryGetValue("projectId", out var raw2)
                    || rd.Values.TryGetValue("id", out raw2))
                {
                    if (int.TryParse(raw2?.ToString(), out var pid2))
                        projectId = pid2;
                }
            }

            if (projectId == null)
            {
                _logger.LogWarning("ProjectAdminHandler: не удалось извлечь projectId из маршрута");
                return;
            }

            _logger.LogInformation("Extracted projectId={ProjectId}", projectId);

            // 2) Берём userId из JWT-claims
            var uid = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(uid, out var userId))
            {
                _logger.LogWarning("ProjectAdminHandler: не удалось извлечь userId из Claims");
                return;
            }

            _logger.LogInformation("Extracted userId={UserId}", userId);

            // 3) Спрашиваем у сервиса, является ли этот пользователь админом проекта
            var isAdmin = await _up.IsAdminAsync(projectId.Value, userId);
            _logger.LogInformation("IsAdminAsync returned {IsAdmin}", isAdmin);

            if (isAdmin)
            {
                _logger.LogInformation("ProjectAdminHandler: requirement succeeded");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogWarning("ProjectAdminHandler: requirement NOT satisfied, returning 403");
            }
        }
    }
}