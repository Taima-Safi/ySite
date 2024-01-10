using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Repository.RepoInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.StaticUserRoles;
using ySite.Service.Authorization.Requirments.CommentRequirements;

namespace ySite.Service.Authorization.Requirments.FriendShipRequirements
{
    public class DeleteFriendRequirements : IAuthorizationRequirement
    {
    }

    public class DeleteFriendRequirementHandler : AuthorizationHandler<DeleteFriendRequirements>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFriendShipRepo _friendRepo;

        public DeleteFriendRequirementHandler(IHttpContextAccessor httpContextAccessor,
            IFriendShipRepo friendRepo)
        {
            _httpContextAccessor = httpContextAccessor;
            _friendRepo = friendRepo;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DeleteFriendRequirements requirement)
        {
            var claims = context.User.Claims;
            var userPermissions = AuthorizeHelper.GetPermissionFromClaim(Contoller.FriendShip, claims);

            // Retrieve the user ID or unique identifier from the claims
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve postId from the route or request body depending on your implementation
            var friendIdValue = _httpContextAccessor.HttpContext.Request.Query["friendId"];
            if (!string.IsNullOrEmpty(friendIdValue))
            {
                string friendId = friendIdValue;
                var relation = await _friendRepo.GetRelation(friendId, userId);
                //if (int.TryParse(friendIdValue, out int friendId))
                //{
                    if (relation != null || (userPermissions is not null && userPermissions.Contains(Permissions.Permission.Delete)))
                    {
                        context.Succeed(requirement);
                    // var IsUserCommentOwner = await IsUserCommentOwnerAsync(userId, friendId);
                    //if (userPermissions is not null && (userPermissions.Contains(Permissions.Permission.Delete))
                    //    {
                    //        context.Succeed(requirement);
                    //    }
                    //    else
                    //    {
                    //        context.Fail();
                    //    }
                    }
                    else
                    {
                        context.Fail();
                    }
                //}
            }
            else
            {
                context.Fail();
            }
        }

        //public async Task<bool> IsUserCommentOwnerAsync(string userId, string friendId)
        //{
        //    var friendShip = await _friendRepo.GetRelation(friendId, userId)
        //    if (friendShip )
        //        return true;
        //    return false;
        //}
    }
}
