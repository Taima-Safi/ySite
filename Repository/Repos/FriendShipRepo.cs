using Microsoft.EntityFrameworkCore;
using Repository.RepoInterfaces;
using ySite.EF.DbContext;
using ySite.EF.Entities;

namespace Repository.Repos
{
    public class FriendShipRepo : IFriendShipRepo
    {
        private readonly AppDbContext _context;

        public FriendShipRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FriendShipModel>> GetRequestList(string userId)
        {
            var friendShips = await _context.FriendShips
                .Include(f => f.User)
                .Include(f => f.Friend)
                .Where(f => f.FriendId == userId
                            && f.Status == FStatus.Pending)
                .ToListAsync();
       
            return friendShips;
        }
        

        public async Task<List<FriendShipModel>> GetUserFriends(string userId)
        {
            var friendShips = await _context.FriendShips
                .Include(f => f.User)
                .Include(f => f.Friend)
                .Where(f => (f.UserId == userId ||f.FriendId == userId)
                            && f.Status == FStatus.Accepted)
                .ToListAsync();
       
            return friendShips;
        }

        public async Task<FriendShipModel> GetRequest(string friendId, string userId)
        {
            var relation =await _context.FriendShips
                .SingleOrDefaultAsync(f => (f.UserId == friendId && f.FriendId == userId)
                                             && f.Status == FStatus.Pending);
            return relation;
        }

        public async Task<FriendShipModel> GetRelation(string friendId, string userId)
        {
            var relation =await _context.FriendShips
                .SingleOrDefaultAsync(f => (f.UserId == userId && f.FriendId == friendId) ||
                                          (f.UserId == friendId && f.FriendId == userId));
            return relation;
        }

        public async Task<bool> AddFriendShip(string friendId, string userId)
        {
            var friendShip = new FriendShipModel
            {
                UserId = userId,
                FriendId = friendId,
                CreatedOn = DateTime.UtcNow,
                Status = 0
            };

           var result = await _context.FriendShips.AddAsync(friendShip);
            if (result == null)
                return false;
            await _context.SaveChangesAsync();
            return true;
        }


        public void updateRelation(FriendShipModel model)
        {

            try
            {
                _context.FriendShips.Update(model);
                _context.SaveChanges();
            }
            catch
            {
                throw new ArgumentException("Can't update this item");
            }
        }
    }
}
