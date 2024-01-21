using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ySite.Core.Dtos.Comments;
using ySite.Core.StaticUserRoles;
using ySite.Service.Interfaces;

namespace ySite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : BaseController
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("GetUserComments")]
        public async Task<IActionResult> GetCommentsAsync()
        {
            var userId = GetUserId();
            return Ok(await _commentService.GetUserComments(userId));
        }
        

        [HttpGet("GetCommentsOnPost")]
        public async Task<IActionResult> GetCommentsOnPostAsync(int postId)
        {
            //var userId = GetUserId();
            return Ok(await _commentService.GetCommentsOnPost(postId));
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentAsync([FromForm] CommentDto dto)
        {
            var userId = GetUserId();
            return Ok(await _commentService.AddComment(dto, userId));
        }

        [HttpDelete]
        //[Authorize(Policy = Policies.DeleteCommentPolicy)]
        public async Task<IActionResult> DeletePostAsync(int commentId)
        {
            var userId = GetUserId();
            return Ok(await _commentService.DeleteComment(commentId, userId));
        }
    }
}