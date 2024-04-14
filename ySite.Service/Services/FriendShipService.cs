using Repository.RepoInterfaces;
using ySite.Core.Dtos.FriendShip;
using ySite.EF.Entities;
using ySite.Service.Interfaces;

namespace ySite.Service.Services;

public class FriendShipService : IFriendShipService
{
    private readonly IFriendShipRepo _friendShipRepo;
    private readonly IAuthRepo _authRepo;

    public FriendShipService(IFriendShipRepo friendShipRepo, IAuthRepo authRepo)
    {
        _friendShipRepo = friendShipRepo;
        _authRepo = authRepo;
    }

    public async Task<FriendShipDto> GetRequests(string userId)
    {
        var dto = new FriendShipDto();
        var user = await _authRepo.FindById(userId);
        if (user is null)
        {
            dto.Message = "invalid User";
            return dto;
        }
        var requests = await _friendShipRepo.GetRequestList(userId);
        if (requests == null)
        {
            dto.Message = "No requests";
            return dto;
        }
        dto.Message = " Your requests :";
        dto.Friends = requests.Select(f => new friendDto
        {
            FriendId = f.UserId,
            UserName = f.User.UserName,
            CreatedOn = f.CreatedOn
        }).ToList();
        return dto;
    }


    public async Task<FriendShipDto> GetUserFriends(string userId)
    {
        var dto = new FriendShipDto();
        var user = await _authRepo.FindById(userId);
        if (user is null)
        {
            dto.Message = "invalid User";
            return dto;
        }

        var friendShips = await _friendShipRepo.GetUserFriends(userId);
        if (friendShips == null)
        {
            dto.Message = "No friends";
            return dto;
        }
        dto.Message = " Your friends :";
        dto.Friends = friendShips.Select(f => new friendDto
        {
            FriendId = f.FriendId == userId ? f.UserId : f.FriendId,
            UserName = f.UserId == userId ? f.Friend.UserName : f.User.UserName,
            CreatedOn = f.CreatedOn
        }).ToList();
        return dto;
    }

    public async Task<string> AcceptFriendship(string friendId, string userId)
    {
        if (await _authRepo.FindById(userId) is null)
            return "invalid User";

        if (await _authRepo.FindById(friendId) is null)
            return "invalid friend";
        var friendShip = await _friendShipRepo.GetRequest(friendId, userId);
        if (friendShip is null)
            return "Invalid Request";
        friendShip.Status = FStatus.Accepted;
        friendShip.UpdatedOn = DateTime.UtcNow;
        _friendShipRepo.updateRelation(friendShip);

        return "You Are Friends Now";
    }


    public async Task<string> RejectFriendship(string friendId, string userId)
    {
        var user = await _authRepo.FindById(userId);
        if (user is null)
            return "invalid User";

        var friend = await _authRepo.FindById(friendId);
        if (friend is null)
            return "invalid friend";

        var friendShip = await _friendShipRepo.GetRequest(friendId, userId);
        if (friendShip == null)
            return "Invalid Request";
        friendShip.Status = FStatus.Declined;
        friendShip.UpdatedOn = DateTime.UtcNow;
        _friendShipRepo.updateRelation(friendShip);

        return $"You Declined a friendship request from {friend.UserName}";
    }

    public async Task<string> AddFriend(string friendId, string userId)
    {
        var user = await _authRepo.FindById(userId);
        if (user is null)
            return "invalid User";

        var friend = await _authRepo.FindById(friendId);
        if (friend is null)
            return "invalid friend";
        var friendShip = await _friendShipRepo.GetRelation(friendId, userId);
        if (friendShip != null)
        {
            if (friendShip.Status == FStatus.Pending)
                return "Your Requist is Pending already";
            if (friendShip.Status == FStatus.Accepted)
                return "You are already Friends";
        }
        if (await _friendShipRepo.AddFriendShip(friendId, userId))
            return $"You sent To {friend.UserName} Friendship Requist";
        return "Can Not Add This user";
    }

    public async Task<string> DeleteFriend(string friendId, string userId)
    {
        var user = await _authRepo.FindById(userId);
        if (user is null)
            return "invalid User";

        var friend = await _authRepo.FindById(friendId);
        if (friend is null)
            return "invalid friend";

        var friendShip = await _friendShipRepo.GetRelation(friendId, userId);
        if (friendShip == null)
            return "Invalid friendShip";

        friendShip.Status = FStatus.Declined;
        friendShip.UpdatedOn = DateTime.UtcNow;
        _friendShipRepo.updateRelation(friendShip);

        return "this friendShip deleted ...";
    }
}
