using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.StaticUserRoles;

namespace ySite.Service.Authorization.Requirments
{
    public class AdminRequirements : IAuthorizationRequirement
    {
    }

    public class AdminRequirementHandler : AuthorizationHandler<AdminRequirements>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirements requirement)
        {
            var claims = context.User.Claims;
            var userPermissions = AuthorizeHelper.GetPermissionFromClaim(Contoller.Post, claims);
       if (userPermissions is not null &&
               userPermissions.Contains(Permissions.Permission.Read) &&
               userPermissions.Contains(Permissions.Permission.Write)
               )     
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