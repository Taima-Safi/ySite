using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.Dtos.Reactions;
using ySite.EF.Entities;

namespace Repository.RepoInterfaces
{
    public interface IPostRepo
    {
        Task<bool> addPostAsync(PostModel p);
        Task<PostModel> GetPostAsync(int postId);
        Task<List<PostModel>> GetPostsAsync(ApplicationUser user);
        void updatePost(PostModel p);
    }
}
