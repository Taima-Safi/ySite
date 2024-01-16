using Microsoft.EntityFrameworkCore;
using Repository.RepoInterfaces;
using ySite.EF.DbContext;
using ySite.EF.Entities;

namespace Repository.Repos
{
    public class CommentRepo : ICommentRepo
    {
        private readonly AppDbContext _context;

        public CommentRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CommentModel> GetCommentAsync(int commentId)
        {
            return await _context.Comments.FindAsync(commentId);
        }

        public async Task<List<CommentModel>> GetCommentsAsync(ApplicationUser user)
        {
            var comments = await _context.Comments.Where(p => p.UserId == user.Id).ToListAsync();

            return comments;
        }

        public async Task<List<CommentModel>> GetCommentsOnPost(int postId)
        {
            var comments = await _context.Comments.Where(r => r.PostId == postId).ToListAsync();
            //var user = await _userManager.FindByNameAsync(userId);
            return comments;
        }
        public async Task<CommentModel?> addCommentAsync(CommentModel comment)
        {
            var commented = await _context.Comments.AddAsync(comment);
            
            if (commented != null)
            {
                _context.SaveChanges();
                return commented.Entity;
            }
            return null;
        }

        public void updateComment(CommentModel comment)
        {
            _context.Comments.Update(comment);
            _context.SaveChanges();
        }
    }
}
