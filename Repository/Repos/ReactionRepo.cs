using Microsoft.EntityFrameworkCore;
using Repository.RepoInterfaces;
using System.Data.Entity;
using ySite.EF.DbContext;
using ySite.EF.Entities;

namespace Repository.Repos
{
    public class ReactionRepo : IReactionRepo
    {
        private readonly AppDbContext _context;

        public ReactionRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReactionModel> GetReaction(int reactionId)
        {
            return await _context.Reactions.FindAsync(reactionId);
        }
        

        public async Task<ReactionModel> GetReactionOfUserOnPost(int postId, string userId)
        {
            return await _context.Reactions.
                Where(r => r.PostId == postId && r.UserId == userId)
                .FirstOrDefaultAsync();
        }
        
        public async Task<List<ReactionModel>> GetReactionsForUser(string userId)
        {
            var reactions = await _context.Reactions.Where(r => r.UserId == userId).ToListAsync();
            //var user = await _userManager.FindByNameAsync(userId);
            return reactions;
        }

        public async Task<List<ReactionModel>> GetReactionsOnPost(int postId)
        {
            var reactions = await _context.Reactions.Where(r => r.PostId == postId).ToListAsync();
            //var user = await _userManager.FindByNameAsync(userId);
            return reactions;
        }
        public async Task<ReactionModel> AddReact(ReactionModel model)
        {
            var reaction = await _context.Reactions.AddAsync(model);
            if (reaction is null)
                return null;
             await _context.SaveChangesAsync();
            return model;
        }

        public void updateReaction(ReactionModel react)
        {

            try
            {
                _context.Reactions.Update(react);
                _context.SaveChanges();
            }
            catch
            {
                throw new ArgumentException("Can't update this item");
            }
        }
    }
}
