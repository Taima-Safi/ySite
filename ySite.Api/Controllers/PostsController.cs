using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ySite.Service.Interfaces;
using ySite.Core.Dtos.Post;
using ySite.Core.StaticUserRoles;
using ySite.Core.Dtos.Posts;
using ySite.EF.Entities;
using ySite.Core.Dtos.Reactions;

namespace ySite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PostsController : BaseController
    {
        private readonly IPostService _postservice;

        public PostsController(IPostService postservice)
        {
            _postservice = postservice;
        }

        [HttpGet("GetUserPosts")]
        public async Task<IActionResult> GetPostsAsync()
        {
            var userId = GetUserId();
            return Ok(await _postservice.GetUserPosts(userId));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPostAsync([FromForm] PostDto dto)
        {
            var userId = GetUserId();
            return Ok(await _postservice.AddPost(dto, userId));
        }
        

        [HttpPut]
        [Authorize(Policy = Policies.EditPostPolicy)]
        public async Task<IActionResult> EditPostAsync([FromForm] UpdatePostDto dto, int postId)
        {
            var userId = GetUserId();
            return Ok(await _postservice.EditPost(dto,userId));
        }

        [HttpDelete]
        [Authorize(Policy = Policies.DeletePostPolicy)]
        public async Task<IActionResult> DeletePostAsync(int postId)
        {
            var userId = GetUserId();
            return Ok(await _postservice.DeletePost(postId, userId));
        }

    }
}
