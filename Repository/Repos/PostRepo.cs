using Microsoft.EntityFrameworkCore;
using Repository.RepoInterfaces;
using ySite.EF.DbContext;
using ySite.EF.Entities;

namespace Repository.Repos
{
    public class PostRepo : IPostRepo
    {
        private readonly AppDbContext _context;

        public PostRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PostModel> GetPostAsync(int postId)
        {
            return await _context.Posts.FindAsync(postId);
        }

        public async Task<List<PostModel>> GetPostsAsync(ApplicationUser user)
        {
            var posts = await _context.Posts.Include(p => p.Reactions)
                .Where(p => p.UserId == user.Id).ToListAsync();

            return posts;
        }

        public async Task<bool> addPostAsync(PostModel p)
        {
            var posted = await _context.Posts.AddAsync(p);
            if (posted != null)
            {
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void updatePost(PostModel p)
        {

            try
            {
                _context.Posts.Update(p);
                _context.SaveChanges();
            }
            catch
            {
                throw new ArgumentException("Can't update this item");
            }
        }


    }
}
