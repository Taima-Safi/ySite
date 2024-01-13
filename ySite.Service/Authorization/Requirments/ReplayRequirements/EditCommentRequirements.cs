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
    public class EditReplayRequirements : IAuthorizationRequirement
    {
    }
    public class EditReplayRequirementHandler : AuthorizationHandler<EditReplayRequirements>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IReplayRepo _replayRepo;

        public EditReplayRequirementHandler(IReplayRepo replayRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _replayRepo = replayRepo;
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EditReplayRequirements requirement)
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
                        var isUserReplayOwner = await IsUserReplayOwnerAsync(userId, replayId);
                        if (userPermissions is not null && isUserReplayOwner)
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
