using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Repository.RepoInterfaces;
using Repository.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.StaticUserRoles;

namespace ySite.Service.Authorization.Requirments.ReactionRequirements
{
    public class DeleteReactionRequirements : IAuthorizationRequirement
    {
    }

    public class DeleteReactionRequirementHandler : AuthorizationHandler<DeleteReactionRequirements>
    {
        private readonly IReactionRepo _reactRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteReactionRequirementHandler(IReactionRepo reactRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _reactRepo = reactRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DeleteReactionRequirements requirement)
        {
            var claims = context.User.Claims;
            // var userPermissions = AuthorizeHelper.GetPermissionFromClaim(Contoller.Comment, claims);
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var reactIdValue = _httpContextAccessor.HttpContext.Request.Query["reactionId"];

            if (!string.IsNullOrEmpty(reactIdValue))
            {
                if (int.TryParse(reactIdValue, out int reactionId))
                {
                    if (await _reactRepo.GetReaction(reactionId) != null)
                    {
                        var isUserReactOwner = await IsUserReactOwnerAsync(userId, reactionId);
                        if (isUserReactOwner)
                            context.Succeed(requirement);
                        else
                            context.Fail();
                    }
                    else
                        context.Fail();
                }
            }
            else
                context.Fail();
        }
        public async Task<bool> IsUserReactOwnerAsync(string userId, int reactionId)
        {
            var react =  await _reactRepo.GetReaction(reactionId);
            if (react.UserId == userId)
                return true;
            return false;
        }
    }
}
