using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ySite.Core.Dtos.Comments;
using ySite.Core.StaticUserRoles;
using ySite.Service.Interfaces;
using ySite.Service.Services;

namespace ySite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendShipsController : BaseController
    {
        private readonly IFriendShipService _friendShipService;

        public FriendShipsController(IFriendShipService friendShipService)
        {
            _friendShipService = friendShipService;
        }



        [HttpGet("GetRequests")]
        public async Task<IActionResult> GetfriendshipRequests()
        {
            var userId = GetUserId();
            return Ok(await _friendShipService.GetRequests(userId));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserFriends()
        {
            var userId = GetUserId();
            return Ok(await _friendShipService.GetUserFriends(userId));
        }

        [HttpPost("AddFriend")]
        public async Task<IActionResult> AddFriendAsync(string friendId)
        {
            var userId = GetUserId();
            return Ok(await _friendShipService.AddFriend(friendId, userId));
        }

        [HttpPost("AcceptFriendshipRequest")]
        public async Task<IActionResult> AcceptFriendshipAsync(string friendId)
        {
            var userId = GetUserId();
            return Ok(await _friendShipService.AcceptFriendship(friendId, userId));
        }
        

        [HttpPost("RejectFriendshipRequest")]
        public async Task<IActionResult> RejectFriendshipAsync(string friendId)
        {
            var userId = GetUserId();
            return Ok(await _friendShipService.RejectFriendship(friendId, userId));
        }

        [HttpDelete]
        [Authorize(Policy = Policies.DeleteFriendPolicy)]

        public async Task<IActionResult> DeleteFriendAsync(string friendId)
        {
            var userId = GetUserId();
            return Ok(await _friendShipService.DeleteFriend(friendId, userId));
        }
    }
}