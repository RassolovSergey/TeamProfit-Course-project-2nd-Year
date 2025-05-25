using Microsoft.AspNetCore.Authorization;

namespace Server.Authorization
{
    /// <summary>Требование: пользователь должен быть администратором проекта</summary>
    public class ProjectAdminRequirement : IAuthorizationRequirement
    {
    }
}
