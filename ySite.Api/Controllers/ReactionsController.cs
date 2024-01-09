using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ySite.Core.Dtos.Reactions;
using ySite.Core.StaticUserRoles;
using ySite.Service.Interfaces;
using ySite.Service.Services;

namespace ySite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionsController : BaseController
    {
        private readonly IReactionService _reactionService;

        public ReactionsController(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        [HttpPost("AddReaction")]
        [Authorize]
        public async Task<IActionResult> ReactPostAsync([FromForm] ReactionDto model)
        {
            var userId = GetUserId();
            return Ok(await _reactionService.React(model, userId));
        }

        [HttpGet("GetReactionsOnPost")]
        [Authorize]
        public async Task<IActionResult> GetReactionsOnPost(int postId)
        {
            var userId = GetUserId();
            return Ok(await _reactionService.GReactsOnPost(postId, userId));
        }

        [HttpDelete("DeleteReactionsOnPost")]
        [Authorize(Policy =Policies.DeleteReactionPolicy)]
        public async Task<IActionResult> DeleteReactionsOnPost(int reactionId, int postId)
        {
            var userId = GetUserId();
            return Ok(await _reactionService.DeleteReact(postId, userId));
        }

        [HttpDelete("DeleteReactbyId")]
        [Authorize(Policy =Policies.DeleteReactionPolicy)]
        public async Task<IActionResult> DeleteReactionsOnPost(int reactionId)
        {
            var userId = GetUserId();
            return Ok(await _reactionService.DeleteReactbyId(reactionId, userId));
        }
    }
}
