using System.Security.Claims;               // Импорт пространства имен для работы с утверждениями (claims) пользователя.
using Microsoft.AspNetCore.Authorization;   // Импорт пространства имен для работы с системой авторизации ASP.NET Core.
using Microsoft.AspNetCore.Mvc.Filters;     // Импорт пространства имен для работы с контекстом фильтров MVC (например, AuthorizationFilterContext).
using Server.Services.Interfaces;           // Импорт пространства имен для интерфейсов сервисов, в частности IUserProjectService.

namespace Server.Authorization
{
    /// <summary>
    /// Handler (обработчик) для проверки требования авторизации <see cref="ProjectAdminRequirement"/>.
    /// Он определяет логику, необходимую для удовлетворения этого требования, а именно:
    /// проверяет, является ли текущий аутентифицированный пользователь администратором указанного проекта.
    /// </summary>
    public class ProjectAdminHandler : AuthorizationHandler<ProjectAdminRequirement>
    {
        // Сервис для работы с проектами и пользователями.
        private readonly IUserProjectService _upService;

        /// <summary>
        /// Конструктор обработчика, использующий внедрение зависимостей для получения экземпляра <see cref="IUserProjectService"/>.
        /// </summary>
        /// <param name="upService">Сервис для проверки пользовательских разрешений в проектах.</param>
        public ProjectAdminHandler(IUserProjectService upService) => _upService = upService; // Инициализация поля _upService полученным экземпляром сервиса.

        /// <summary>
        /// Асинхронный метод, который содержит основную логику проверки требования авторизации.
        /// Вызывается системой авторизации ASP.NET Core для каждого требования <see cref="ProjectAdminRequirement"/>.
        /// </summary>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectAdminRequirement requirement)
        {
            // Проверяем, является ли ресурс, к которому применяется авторизация, контекстом фильтрации MVC.
            // Это позволяет получить доступ к данным маршрута (например, {projectId} из URL).
            if (context.Resource is AuthorizationFilterContext mvcCtx &&
                // Пытаемся извлечь значение "projectId" из данных маршрута.
                mvcCtx.RouteData.Values.TryGetValue("projectId", out var pid) &&
                // Пытаемся преобразовать полученное значение projectId в целое число.
                int.TryParse(pid?.ToString(), out var projectId))
            {
                // Извлекаем идентификатор пользователя (UID) из утверждений (claims) аутентифицированного пользователя.
                // ClaimTypes.NameIdentifier обычно содержит уникальный идентификатор пользователя.
                var uid = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Если идентификатор пользователя успешно преобразован в целое число
                // И сервис _upService подтверждает, что пользователь является администратором данного проекта,
                if (int.TryParse(uid, out var userId) &&
                    await _upService.IsAdminAsync(projectId, userId))
                {
                    // То требование авторизации считается выполненным.
                    // Пользователю разрешается доступ к ресурсу.
                    context.Succeed(requirement);
                }
            }
            // Если условия не выполнены (например, нет projectId в маршруте,
            // или пользователь не является администратором), метод завершается,
            // и требование не будет удовлетворено по умолчанию, что приведет к отказу в доступе.
        }
    }
}