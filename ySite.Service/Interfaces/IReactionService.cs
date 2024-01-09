using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.Dtos.Reactions;
using ySite.EF.Entities;

namespace ySite.Service.Interfaces
{
    public interface IReactionService
    {
        Task<ReadReactionDto> React(ReactionDto dto, string userId);

        Task<ReactsOnPostResultDto> GReactsOnPost(int postId, string userId);
        Task<string> DeleteReactbyId(int reactId, string userId);
        Task<string> DeleteReact(int postId, string userId);
    }
}
