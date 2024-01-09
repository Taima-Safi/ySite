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

namespace ySite.Service.Authorization.Requirments.PostRequirements
{
    public class EditPostRequirements : IAuthorizationRequirement
    {
    }

    public class EditPostRequirementHandler : AuthorizationHandler<EditPostRequirements>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostRepo _postRepo;

        public EditPostRequirementHandler(IHttpContextAccessor httpContextAccessor,
            IPostRepo postRepo)
        {
            _httpContextAccessor = httpContextAccessor;
            _postRepo = postRepo;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EditPostRequirements requirement)
        {
            var claims = context.User.Claims;
            var userPermissions = AuthorizeHelper.GetPermissionFromClaim(Contoller.Post, claims);

            // Retrieve the user ID or unique identifier from the claims
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve postId from the route or request body depending on your implementation
            var postIdValue = _httpContextAccessor.HttpContext.Request.Query["postId"];
            if (!string.IsNullOrEmpty(postIdValue))
            {
                if (int.TryParse(postIdValue, out int postId))
                {
                    if (await _postRepo.GetPostAsync(postId) != null)
                    {
                        var isUserPostOwner = await IsUserPostOwnerAsync(userId, postId);
                        if (userPermissions is not null && isUserPostOwner)
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

        public async Task<bool> IsUserPostOwnerAsync(string userId, int postId)
        {
            var post = await _postRepo.GetPostAsync(postId);
            if (post.UserId == userId)
                return true;
            return false;
        }
    }
}
