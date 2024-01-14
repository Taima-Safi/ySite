using Microsoft.AspNetCore.Http;
using Repository.RepoInterfaces;
using ySite.Core.Dtos.Post;
using ySite.Core.Dtos.Posts;
using ySite.EF.Entities;
using ySite.Service.Interfaces;

namespace ySite.Service.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepo _postRepo;
        private readonly IAuthRepo _authRepo;
        private readonly IReactionRepo _reactionRepo;
        private readonly ICommentRepo _commentRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(IPostRepo postRepo, IAuthRepo authRepo,
            IHttpContextAccessor httpContextAccessor, IReactionRepo reactionRepo,
            ICommentRepo commentRepo)
        {
            _postRepo = postRepo;
            _authRepo = authRepo;
            _httpContextAccessor = httpContextAccessor;
            _reactionRepo = reactionRepo;
            _commentRepo = commentRepo;
        }

        public async Task<UserPostsResultDto> GetUserPosts(string userId)
        {
            var userPostsR = new UserPostsResultDto();
            if (userId is null)
            {
                userPostsR.Message = "UserId invalid!";
                return userPostsR;
            }
            var user = await _authRepo.FindById(userId);
            if(user is null)
            {
                userPostsR.Message = "User Is Not Found!";
                return userPostsR;
            }
            var posts = await _postRepo.GetPostsAsync(user);
            if(posts is null || !posts.Any())
            {
                userPostsR.Message = "This user does not have any posts";
                return userPostsR;
            }
            userPostsR.Message = $"The Posts for {user.FirstName} are these....";
            userPostsR.Posts = posts.Select(post => new UserPostsDto
            {
                Description = post.Description,
                Image = post.Image,
                CommentsCount = post.CommentsCount,
                ReactsCount = post.ReactsCount
            }).ToList();

            return userPostsR;
        }

        public async Task<bool> AddPost(PostDto dto, string userId)
        {
            if (dto == null || userId == null || (dto.Description == null && dto.ClientFile == null))
            {
                return false;
            }
            var post = new PostModel();
            post.UserId = userId;
            if (dto.Description != null)
                post.Description = dto.Description;

            if (dto.ClientFile != null)
            {
                using var datastream = new MemoryStream();
                await dto.ClientFile.CopyToAsync(datastream);

                post.Image = datastream.ToArray();
            }
            if (await _postRepo.addPostAsync(post))
                return true;

            return false;
        }

        public async Task<string> EditPost(UpdatePostDto dto, string userId)
        {
            if (dto == null)
                return "";

            var post = await _postRepo.GetPostAsync(dto.Id);
            if (post == null)
                return "Invalid Post";

            if (dto.Description != null)
                post.Description = dto.Description;

            if (dto.ClientFile != null)
            {
                using var datastream = new MemoryStream();
                await dto.ClientFile.CopyToAsync(datastream);

                post.Image = datastream.ToArray();
            }
            post.UpdatedOn = DateTime.UtcNow;
            _postRepo.updatePost(post);

            return "Post Updated...";
        }

        public async Task<string> DeletePost(int postId, string userId)
        {
            var user =await _authRepo.FindById(userId);
            if (user is null)
                return "Invalid User";
            
            var post =await _postRepo.GetPostAsync(postId);
            if (post is null)
                return "Invalid Post";
            var reactions = await _reactionRepo.GetReactionsOnPost(postId);
            var comments = await _commentRepo.GetCommentsOnPost(postId);
            post.IsDeleted = true;
            post.DeletedOn = DateTime.UtcNow;

            foreach (var reaction in reactions)
            {
                reaction.IsDeleted = true;
                reaction.DeletedOn = DateTime.UtcNow;
            }
            foreach (var comment in comments)
            {
                comment.IsDeleted = true;
                comment.DeletedOn = DateTime.UtcNow;
            }
            
            _postRepo.updatePost(post);

            return "this post deleted ...";
        }
    }
}