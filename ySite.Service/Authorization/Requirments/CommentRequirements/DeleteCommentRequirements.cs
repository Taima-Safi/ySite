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
using ySite.Service.Authorization.Requirments.PostRequirements;

namespace ySite.Service.Authorization.Requirments.CommentRequirements
{
    public class DeleteCommentRequirements : IAuthorizationRequirement
    {
    }

    public class DeleteCommentRequirementHandler : AuthorizationHandler<DeleteCommentRequirements>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommentRepo _commentRepo;

        public DeleteCommentRequirementHandler(IHttpContextAccessor httpContextAccessor,
            ICommentRepo commentRepo)
        {
            _httpContextAccessor = httpContextAccessor;
            _commentRepo = commentRepo;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DeleteCommentRequirements requirement)
        {
            var claims = context.User.Claims;
            var userPermissions = AuthorizeHelper.GetPermissionFromClaim(Contoller.Comment, claims);

            // Retrieve the user ID or unique identifier from the claims
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Retrieve postId from the route or request body depending on your implementation
            var commentIdValue = _httpContextAccessor.HttpContext.Request.Query["commentId"];
            if (!string.IsNullOrEmpty(commentIdValue))
            {
                if (int.TryParse(commentIdValue, out int commentId))
                {
                    if (await _commentRepo.GetCommentAsync(commentId) != null)
                    {
                        var IsUserCommentOwner = await IsUserCommentOwnerAsync(userId, commentId);
                        if (userPermissions is not null &&
                        (userPermissions.Contains(Permissions.Permission.Delete) ||
                         IsUserCommentOwner))
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

        public async Task<bool> IsUserCommentOwnerAsync(string userId, int commentId)
        {
            var comment = await _commentRepo.GetCommentAsync(commentId);
            if (comment.UserId == userId)
                return true;
            return false;
        }
    }
}
