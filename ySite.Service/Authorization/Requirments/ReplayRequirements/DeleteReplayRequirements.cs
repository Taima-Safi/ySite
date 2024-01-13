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

namespace ySite.Service.Authorization.Requirments.ReplayRequirements
{
    public class DeleteReplayRequirements : IAuthorizationRequirement
    {
    }

    public class DeleteReplayRequirementHandler : AuthorizationHandler<DeleteReplayRequirements>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IReplayRepo _replayRepo;

        public DeleteReplayRequirementHandler(IHttpContextAccessor httpContextAccessor,
            IReplayRepo replayRepo)
        {
            _httpContextAccessor = httpContextAccessor;
            _replayRepo = replayRepo;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DeleteReplayRequirements requirement)
        {
            var claims = context.User.Claims;
            var userPermissions = AuthorizeHelper.GetPermissionFromClaim(Contoller.Replay, claims);

            // Retrieve the user ID or unique identifier from the claims
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve postId from the route or request body depending on your implementation
            var replayIdValue = _httpContextAccessor.HttpContext.Request.Query["replayId"];
            if (!string.IsNullOrEmpty(replayIdValue))
            {
                if (int.TryParse(replayIdValue, out int replayId))
                {
                    if (await _replayRepo.GetReplay(replayId) != null)
                    {
                        var IsUserReplayOwner = await IsUserReplayOwnerAsync(userId, replayId);
                        if (userPermissions is not null &&
                        (userPermissions.Contains(Permissions.Permission.Delete) ||
                         IsUserReplayOwner))
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            context.Fail();
                        }
                    }
                    else
                    {
                        context.Fail();
                    }
                }
            }
            else
            {
                context.Fail();
            }
        }

        public async Task<bool> IsUserReplayOwnerAsync(string userId, int replayId)
        {
            var replay = await _replayRepo.GetReplay(replayId);
            if (replay.UserId == userId)
                return true;
            return false;
        }
    }
}
