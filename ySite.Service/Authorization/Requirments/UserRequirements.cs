using Microsoft.AspNetCore.Authorization;
using ySite.Core.StaticUserRoles;

namespace ySite.Service.Authorization.Requirments;

public class UserRequirements : IAuthorizationRequirement
{
}

public class UserRequirementHandler : AuthorizationHandler<UserRequirements>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirements requirement)
    {
        var claims = context.User.Claims;

        var userPermissions = AuthorizeHelper.GetPermissionFromClaim(Contoller.Post, claims);

        if (userPermissions is not null &&
           userPermissions.Contains(Permissions.Permission.Read)
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
