using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.StaticUserRoles;

namespace ySite.Service.Authorization.Requirments
{
    public class OwnerRequirements : IAuthorizationRequirement
    {
    }

    public class OwnerRequirementsHandler : AuthorizationHandler<OwnerRequirements>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerRequirements requirement)
        {
            if (context.User.IsInRole(UserRoles.OWNER))
            {
                context.Succeed(requirement);
            }
            else
            {
              context.Fail();
            }
        }
    }
    //protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerRequirements requirement)
    //{
    //    var claim = context.User.Claims;
    //    var userpermissions = AuthorizeHelper.GetPermissionFromClaim(Contoller.Post, claim);

    //    if(userpermissions is not null &&
    //        userpermissions.Contains(Permissions.Permission.Read)&&
    //        userpermissions.Contains(Permissions.Permission.Write)&&
    //        userpermissions.Contains(Permissions.Permission.Update)&&
    //        userpermissions.Contains(Permissions.Permission.Delete))
    //    {
    //        context.Succeed(requirement);
    //    }
    //    else
    //    {
    //        context.Fail();
    //    }
    //    return Task.CompletedTask;
    //}
//}
}
