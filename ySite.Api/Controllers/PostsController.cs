using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ySite.Core.Dtos.Post;
using ySite.Core.Dtos.Posts;
using ySite.Service.Interfaces;

namespace ySite.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsController : BaseController
{
    private readonly IPostService _postservice;
    private readonly IStringLocalizer<PostsController> _localization;

    public PostsController(IPostService postservice,
        IStringLocalizer<PostsController> localization)
    {
        _postservice = postservice;
        _localization = localization;
    }

    [HttpGet("GetUserPosts")]
    public async Task<IActionResult> GetPostsAsync()
    {
        var userId = GetUserId();
        return Ok(await _postservice.GetUserPosts(userId));
    }

    [HttpPost]
    public async Task<IActionResult> AddPostAsync([FromForm] PostDto dto)
    {
        var userId = GetUserId();
        return Ok(await _postservice.AddPost(dto, userId));
    }

    [HttpPut]
    //[Authorize(Policy = Policies.EditPostPolicy)]
    public async Task<IActionResult> EditPostAsync([FromForm] UpdatePostDto dto, int postId)
    {
        var userId = GetUserId();
        return Ok(await _postservice.EditPost(dto, userId));
    }

    [HttpDelete]
    // [Authorize(Policy = Policies.DeletePostPolicy)]
    public async Task<IActionResult> DeletePostAsync(int postId)
    {
        var userId = GetUserId();
        return Ok(await _postservice.DeletePost(postId, userId));
    }
}
