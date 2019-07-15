using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using Etutor.BL.Authorization.Requeriments;
using Etutor.Core.Models.Enums;

namespace Etutor.Api.Filters
{
    public class RequiresPermissionFilterAttribute : TypeFilterAttribute
    {
        public RequiresPermissionFilterAttribute(OperationsPermission permissions)
            : base(typeof(RequiresPermissionFilterAttributeImpl))
        {
            Arguments = new object[] { new AccessControllerRequirement(permissions) };
        }
    }

    public class RequiresPermissionFilterAttributeImpl : Attribute, IAsyncResourceFilter
    {
        private readonly IAuthorizationService _authService;
        private readonly AccessControllerRequirement _requiredPermissions;

        public RequiresPermissionFilterAttributeImpl(IAuthorizationService authService, AccessControllerRequirement requiredPermissions)
        {
            _authService = authService;
            _requiredPermissions = requiredPermissions;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context,
            ResourceExecutionDelegate next)
        {
            var authResult = await _authService.AuthorizeAsync(context.HttpContext.User, context, _requiredPermissions);
            if (!authResult.Succeeded)
            {
                context.Result = /*new ChallengeResult();*/ new UnauthorizedResult();
                await context.Result.ExecuteResultAsync(context);
            }
            else
            {
                await next();
            }
        }
    }
}
