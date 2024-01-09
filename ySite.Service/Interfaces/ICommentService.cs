using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.Dtos.Comments;

namespace ySite.Service.Interfaces
{
    public interface ICommentService
    {
        Task<ReadCommentDto> AddComment(CommentDto dto, string userId);
        Task<string> DeleteComment(int commentId, string userId);
        Task<UserCommentsResultDto> GetCommentsOnPost(int postId);
        Task<UserCommentsResultDto> GetUserComments(string userId);
    }
}
