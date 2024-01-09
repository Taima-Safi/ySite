using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.Dtos.Post;
using ySite.Core.Dtos.Posts;
using ySite.Core.Dtos.Reactions;
using ySite.EF.Entities;

namespace ySite.Service.Interfaces
{
    public interface IPostService
    {
        Task<bool> AddPost(PostDto dto, string userId);
        Task<UserPostsResultDto> GetUserPosts(string userId);
        Task<string> DeletePost(int postId, string userId);
        Task<string> EditPost(UpdatePostDto dto, string userId);
    }
}
