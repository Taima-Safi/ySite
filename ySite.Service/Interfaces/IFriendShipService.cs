using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.Dtos.FriendShip;

namespace ySite.Service.Interfaces
{
    public interface IFriendShipService
    {
        Task<string> AcceptFriendship(string friendId, string userId);
        Task<string> AddFriend(string friendId, string userId);
        Task<string> DeleteFriend(string friendId, string userId);
        Task<FriendShipDto> GetRequests(string userId);
        Task<FriendShipDto> GetUserFriends(string userId);
        Task<string> RejectFriendship(string friendId, string userId);
    }
}
