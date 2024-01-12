using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.EF.Entities;

namespace Repository.RepoInterfaces
{
    public interface IReplayRepo
    {
        Task<ReplayModel?> addReplayAsync(ReplayModel model);
        Task<ReplayModel> GetReplay(int replayId);
        Task<List<ReplayModel>> GetReplaysOnComment(int commentId);
        void updateReplay(ReplayModel replay);
    }
}
