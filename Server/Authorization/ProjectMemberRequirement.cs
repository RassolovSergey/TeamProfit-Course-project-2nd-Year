using Microsoft.AspNetCore.Authorization;

namespace Server.Authorization
{
    /// <summary>Требование: пользователь должен состоять в проекте</summary>
    public class ProjectMemberRequirement : IAuthorizationRequirement
    {
    }
}
