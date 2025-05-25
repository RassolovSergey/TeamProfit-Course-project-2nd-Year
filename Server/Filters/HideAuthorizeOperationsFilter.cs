using Microsoft.AspNetCore.Authorization;   // Импорт пространства имен для атрибута [Authorize].
using Microsoft.OpenApi.Models;             // Импорт пространства имен для работы с моделями OpenAPI (Swagger).
using Swashbuckle.AspNetCore.SwaggerGen;    // Импорт пространства имен для интерфейса IOperationFilter.

namespace Server.Filters
{
    /// <summary>
    /// Фильтр операций Swagger, предназначенный для скрытия или изменения отображения методов API,
    /// которые защищены атрибутом [Authorize].
    /// </summary>
    public class HideAuthorizeOperationsFilter : IOperationFilter
    {
        /// <summary>
        /// Применяет логику фильтрации к каждой операции (методу) API в документации Swagger.
        /// </summary>
        /// <param name="operation">Объект OpenApiOperation, представляющий текущую операцию API.</param>
        /// <param name="context">Контекст фильтрации операции, предоставляющий информацию о методе и контроллере.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Проверяем, есть ли атрибут [Authorize] на самом методе контроллера
            // ИЛИ на классе контроллера, к которому принадлежит метод.
            // .GetCustomAttributes(true) - получает все пользовательские атрибуты, включая унаследованные.
            // .OfType<AuthorizeAttribute>() - фильтрует атрибуты, оставляя только [Authorize].
            // .Any() - проверяет, есть ли хотя бы один такой атрибут.
            var hasAuthorize = context.MethodInfo.DeclaringType! // Доступ к типу (классу контроллера), в котором объявлен метод.
                                    .GetCustomAttributes(true)
                                    .OfType<AuthorizeAttribute>().Any()
                               || context.MethodInfo // Доступ к информации о самом методе.
                                    .GetCustomAttributes(true)
                                    .OfType<AuthorizeAttribute>().Any();

            // Если метод или его контроллер помечены атрибутом [Authorize], значит, он требует авторизации.
            if (hasAuthorize)
            {
                // Пропимываем "(Недоступен)" на методах недоступных без авторизации.
                operation.Summary = "(Недоступен)";
            }
        }
    }
}