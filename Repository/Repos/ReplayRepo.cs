
using Repository.RepoInterfaces;
using ySite.EF.DbContext;
using ySite.EF.Entities;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repos
{
    public class ReplayRepo : IReplayRepo
    {
        private readonly AppDbContext _context;

        public ReplayRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReplayModel> GetReplay(int replayId)
        {
            var replay = await _context.Replays.FindAsync(replayId);
            return replay;
        }
        
        public async Task<List<ReplayModel>> GetReplaysOnComment(int commentId)
        {
            var replays= await _context.Replays.Where(r => r.CommentId == commentId)
                .ToListAsync();
            return replays;
        }

        public async Task<ReplayModel?> addReplayAsync(ReplayModel model)
        {
            var replayed = await _context.Replays.AddAsync(model);

            if (replayed != null)
            {
                _context.SaveChanges();
                return replayed.Entity;
            }
            return null;
        }

        public void updateReplay(ReplayModel replay)
        {

            try
            {
                _context.Replays.Update(replay);
                _context.SaveChanges();
            }
            catch
            {
                throw new ArgumentException("Can't update this item");
            }
        }
    }
}
