using ySite.Core.Dtos.Replays;

namespace ySite.Service.Interfaces
{
    public interface IReplayService
    {
        Task<ReadReplayDto> AddReplay(ReplayDto dto, string userId);
        Task<string> DeleteReplay(int replayId, string userId);
        Task<ReadReplayDto> EditReplay(EditReplayDto dto, string userId);
        Task<ReplaysOnCommentResultDto> getReplaysOnComment(int commentId);
    }
}
