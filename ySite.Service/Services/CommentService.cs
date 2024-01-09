using Repository.RepoInterfaces;
using ySite.Core.Dtos.Comments;
using ySite.EF.Entities;
using ySite.Service.Interfaces;

namespace ySite.Service.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepo _commentRepo;
        private readonly IPostRepo _postRepo;
        private readonly IAuthRepo _authRepo;

        public CommentService(ICommentRepo commentRepo, IPostRepo postRepo,
            IAuthRepo authRepo)
        {
            _commentRepo = commentRepo;
            _postRepo = postRepo;
            _authRepo = authRepo;
        }

        public async Task<UserCommentsResultDto> GetUserComments(string userId)
        {
            var userCommentsR = new UserCommentsResultDto();
            var user = await _authRepo.FindById(userId);
            if (user is null)
            {
                userCommentsR.Message = "invalid User";
                return userCommentsR;
            }
            var comments =await _commentRepo.GetCommentsAsync(user);

            if (comments is null || !comments.Any())
            {
                userCommentsR.Message = $"This user {user.FirstName} Does not has any comment yet!..";
                return userCommentsR;
            }
            userCommentsR.Comments = comments.Select(c => new ReadCommentDto
            {
                Message = $"Comments of {c.User.FirstName} : ",
                Comment = c.Comment,
                PostId = c.PostId,
                ClientFile = c.Image,
                CreatedOn = c.CreatedOn,
                UserId = c.UserId,
                Id = c.Id
            }).ToList();

            return userCommentsR;
        }

        public async Task<UserCommentsResultDto> GetCommentsOnPost(int postId)
        {
            var commentR = new UserCommentsResultDto();
            var post = await _postRepo.GetPostAsync(postId);
            if (post is null)
            {
                commentR.Message = "invalid Post";
                return commentR;
            }
            var comments = await _commentRepo.GetCommentsOnPost(postId);
            if(comments == null)
            {
                commentR.Message = "No Comments on this post";
                return commentR;
            }
            commentR.Message = $"This is All Comments on {post.Id}";
            commentR.Comments = comments.Select(c => new ReadCommentDto
            {
                Id = c.Id,
                Comment = c.Comment,
                ClientFile = c.Image,
                PostId = c.PostId,
                UserId = c.UserId,
                CreatedOn = c.CreatedOn,

            }).ToList();
            return commentR;
        }

        public async Task<ReadCommentDto> AddComment(CommentDto dto, string userId)
        {
            var commentR = new ReadCommentDto();
            var user = await _authRepo.FindById(userId);
            if(user is null)
            {
                commentR.Message = "invalid User";
                return commentR;
            }

            var post = await _postRepo.GetPostAsync(dto.PostId);
            if (post is null)
            {
                commentR.Message = "invalid Post";
                return commentR;
            }
            if(dto == null || (dto.Comment == null && dto.ClientFile == null))
            {
                commentR.Message = "Can not add Empty comment ";
                return commentR;
            }
            var comment = new CommentModel();
            if(dto.Comment is not null)
                comment.Comment = dto.Comment;

            comment.PostId = dto.PostId;
            comment.UserId = userId;
            comment.CreatedOn = DateTime.Now;

            if (dto.ClientFile != null)
            {
                using var datastream = new MemoryStream();
                await dto.ClientFile.CopyToAsync(datastream);

                comment.Image = datastream.ToArray();
            }
            var commented = await _commentRepo.addCommentAsync(comment);
            if (commented != null)
            {
                commentR.Message = "Commented Successfuly..";
                commentR.Id = commented.Id;
                commentR.PostId = commented.PostId;
                commentR.UserId = userId;
                commentR.Comment = commented.Comment;
                commentR.CreatedOn = commented.CreatedOn;
                commentR.ClientFile = commented.Image;

                return commentR;
            }
            commentR.Message = "Can not Comment!..";
            return commentR;
        }

        public async Task<string> DeleteComment(int commentId, string userId)
        {
            var user = await _authRepo.FindById(userId);
            if (user is null)
                return "Invalid User";

            var comment = await _commentRepo.GetCommentAsync(commentId);
            if (comment is null)
                return "Invalid Comment";

            comment.IsDeleted = true;
            comment.DeletedOn = DateTime.UtcNow;
            _commentRepo.updateComment(comment);

            return "this comment deleted ...";
        }
    }
}
