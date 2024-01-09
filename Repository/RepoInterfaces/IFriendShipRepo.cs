using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.EF.Entities;

namespace Repository.RepoInterfaces
{
    public interface IFriendShipRepo
    {
        Task<bool> AddFriendShip(string friendId, string userId);
        //Task<FriendShipModel> GetRelation(string friendId, string userId);
        Task<FriendShipModel> GetRelation(string friendId, string userId);
        Task<FriendShipModel> GetRequest(string friendId, string userId);
        Task<List<FriendShipModel>> GetRequestList(string userId);
        Task<List<FriendShipModel>> GetUserFriends(string userId);
        void updateRelation(FriendShipModel model);
    }
}
