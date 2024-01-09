using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.StaticUserRoles;

namespace ySite.Service.Authorization.Requirments
{
    public class GenericOwnerRequirements : IAuthorizationRequirement
    {
    }

    public class GenericOwnerRequirementHandler : AuthorizationHandler<GenericOwnerRequirements>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GenericOwnerRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GenericOwnerRequirements requirement)
        {
            var controllerName = _httpContextAccessor.HttpContext.GetRouteData().Values["controller"]?.ToString();
            var actionName = _httpContextAccessor.HttpContext.GetRouteData().Values["action"]?.ToString();
            var requieredPermission = AuthorizeHelper.GetActionPermission(actionName);
         //   var resourceIdValue = _httpContextAccessor.HttpContext.Request.Query["controllerNameId"];

            var user = context.User;
            var claims = user.Claims;

            var userPermissions = AuthorizeHelper.GetPermissionFromClaim(controllerName, claims);
            if (userPermissions is not null &&
                requieredPermission != Permissions.Permission.None &&
                userPermissions.Contains(requieredPermission))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
