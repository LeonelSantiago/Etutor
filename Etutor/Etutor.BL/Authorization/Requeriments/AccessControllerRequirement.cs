using Microsoft.AspNetCore.Authorization;
using Etutor.Core.Models.Enums;

namespace Etutor.BL.Authorization.Requeriments
{
    public class AccessControllerRequirement : IAuthorizationRequirement
    {
        public OperationsPermission RequiredPermissions { get; }

        public AccessControllerRequirement()
        {
            RequiredPermissions = OperationsPermission.None;
        }

        public AccessControllerRequirement(OperationsPermission requiredPermissions)
        {
            RequiredPermissions = requiredPermissions;
        }
    }
}
