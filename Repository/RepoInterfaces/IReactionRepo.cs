using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.Dtos.Reactions;
using ySite.EF.Entities;

namespace Repository.RepoInterfaces
{
    public interface IReactionRepo
    {
        Task<List<ReactionModel>> GetReactionsForUser(string userId);
        Task<List<ReactionModel>> GetReactionsOnPost(int postId);
        Task<ReactionModel> AddReact(ReactionModel model);
        void updateReaction(ReactionModel react);
        Task<ReactionModel> GetReaction(int reactionId);
        Task<ReactionModel> GetReactionOfUserOnPost(int postId, string userId);
    }
}
