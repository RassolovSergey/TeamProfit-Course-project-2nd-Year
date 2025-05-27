// ProjectMemberHandler.cs
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Server.Services.Interfaces;

namespace Server.Authorization
{
    /// <summary>Handler для проверки, что пользователь – участник проекта</summary>
    public class ProjectMemberHandler
        : AuthorizationHandler<ProjectMemberRequirement>
    {
        private readonly IUserProjectService _up;
        private readonly ILogger<ProjectMemberHandler> _logger;

        public ProjectMemberHandler(
            IUserProjectService up,
            ILogger<ProjectMemberHandler> logger)
        {
            _up = up;
            _logger = logger;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ProjectMemberRequirement requirement)
        {
            _logger.LogInformation("ProjectMemberHandler invoked. Resource={Resource}",
                context.Resource?.GetType().Name);

            int? projectId = null;

            // MVC-filters
            if (context.Resource is AuthorizationFilterContext mvcCtx)
            {
                if (mvcCtx.RouteData.Values.TryGetValue("projectId", out var raw1)
                 || mvcCtx.RouteData.Values.TryGetValue("id", out raw1))
                {
                    if (int.TryParse(raw1?.ToString(), out var pid1))
                        projectId = pid1;
                }
            }
            // endpoint routing
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
                _logger.LogWarning("MemberHandler: projectId not found in route");
                return;
            }

            _logger.LogInformation("MemberHandler: projectId={ProjectId}", projectId);

            var uid = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(uid, out var userId))
            {
                _logger.LogWarning("MemberHandler: userId claim invalid");
                return;
            }

            _logger.LogInformation("MemberHandler: userId={UserId}", userId);

            if (await _up.IsMemberAsync(projectId.Value, userId))
            {
                _logger.LogInformation("MemberHandler: requirement succeeded");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogWarning("MemberHandler: requirement not satisfied");
            }
        }
    }
}
