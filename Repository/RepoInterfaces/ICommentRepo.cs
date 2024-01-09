using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.EF.Entities;

namespace Repository.RepoInterfaces
{
    public interface ICommentRepo
    {
        Task<CommentModel?> addCommentAsync(CommentModel comment);
        Task<CommentModel> GetCommentAsync(int commentId);
        Task<List<CommentModel>> GetCommentsAsync(ApplicationUser user);
        Task<List<CommentModel>> GetCommentsOnPost(int postId);
        void updateComment(CommentModel comm);
    }
}
