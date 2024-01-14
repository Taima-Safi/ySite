using Repository.RepoInterfaces;
using Repository.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ySite.Core.Dtos.Comments;
using ySite.Core.Dtos.Reactions;
using ySite.EF.Entities;
using ySite.Service.Interfaces;

namespace ySite.Service.Services
{
    public class ReactionService : IReactionService
    {
        private readonly IReactionRepo _reactionRepo;
        private readonly IPostRepo _postRepo;
        private readonly IAuthRepo _authRepo;

        public ReactionService(IReactionRepo reactionRepo,
            IPostRepo postRepo, IAuthRepo authRepo)
        {
            _reactionRepo = reactionRepo;
            _postRepo = postRepo;
            _authRepo = authRepo;
        }
        public async Task<ReactsOnPostResultDto> GReactsOnPost(int postId, string userId)
        {
            var reactionsR = new ReactsOnPostResultDto();
            var user = await _authRepo.FindById(userId);
            if (user is null)
            {
                reactionsR.Message = "invalid User";
                return reactionsR;
            }

            var post = await _postRepo.GetPostAsync(postId);
            if (post is null)
            {
                reactionsR.Message = "invalid Post";
                return reactionsR;
            }
            var reactions = await _reactionRepo.GetReactionsOnPost(postId);
            if (reactions == null)
            {
                reactionsR.Message = "There is no reactions on this post!..";
                return reactionsR;
            }
            reactionsR.Reactions = reactions.Select(r => new ReactsOnPostDto
            {
                Reaction = (int)r.Reaction,
                UserId = r.UserId,
                CreatedOn = r.CreatedOn
            }).ToList();
            return reactionsR;

        }
        public async Task<ReadReactionDto> React(ReactionDto dto, string userId)
        {
            var reactionR = new ReadReactionDto();

            if (dto == null)
            {
                reactionR.Message = "No Reaction inserted! .";
                return reactionR;
            }
            var user = await _authRepo.FindById(userId);
            if (user is null)
            {
                reactionR.Message = "invalid User";
                return reactionR;
            }

            var post = await _postRepo.GetPostAsync(dto.PostId);
            if (post is null)
            {
                reactionR.Message = "invalid Post";
                return reactionR;
            }
            var reaction = new ReactionModel
            {
                Reaction = (ReactionType)dto.Reaction,
                PostId = dto.PostId,
                UserId = userId
            };
            var testR = await _reactionRepo.GetReactionsOnPost(post.Id);
            foreach (var r in testR)
            {
                if (r.UserId == userId)
                {
                    r.IsDeleted = true;
                    r.DeletedOn = DateTime.UtcNow;
                    post.ReactsCount--;

                }
            }
            var reacted = await _reactionRepo.AddReact(reaction);
            if (reacted is null)
            {
                reactionR.Message = "Cannot React On this Post";
                return reactionR;
            }
            post.ReactsCount++;
            _postRepo.updatePost(post);

            reactionR.Message = $"Reacted on {user.FirstName} Post successfuly..";
            reactionR.Reaction = dto.Reaction;
            reactionR.UserId = userId;
            reactionR.PostId = dto.PostId;
            return reactionR;
        }

        public async Task<string> DeleteReact(int postId, string userId)
        {
            var user = await _authRepo.FindById(userId);
            if (user is null)
                return "Invalid User";

            var reaction = await _reactionRepo.GetReactionOfUserOnPost(postId, userId);
            if (reaction is null)
                return "Invalid reaction";
            var post = await _postRepo.GetPostAsync(reaction.PostId);

            reaction.IsDeleted = true;
            reaction.DeletedOn = DateTime.UtcNow;
            post.ReactsCount--;

            _reactionRepo.updateReaction(reaction);
            _postRepo.updatePost(post);

            return "this Reaction deleted ...";
        }

        public async Task<string> DeleteReactbyId(int reactId, string userId)
        {
            var user = await _authRepo.FindById(userId);
            if (user is null)
                return "Invalid User";

            var reaction = await _reactionRepo.GetReaction(reactId);
            if (reaction is null)
                return "Invalid reaction";
            var post = await _postRepo.GetPostAsync(reaction.PostId);

            reaction.IsDeleted = true;
            reaction.DeletedOn = DateTime.UtcNow;
            post.ReactsCount--;

            _reactionRepo.updateReaction(reaction);
            _postRepo.updatePost(post);

            return "this Reaction deleted ...";
        }
    }

}
