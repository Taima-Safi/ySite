using Repository.RepoInterfaces;
using ySite.Core.Dtos.Replays;
using ySite.EF.Entities;
using ySite.Service.Interfaces;

namespace ySite.Service.Services;

public class ReplayService : IReplayService
{
    private readonly IReplayRepo _replayRepo;
    private readonly ICommentRepo _commentRepo;
    private readonly IAuthRepo _authRepo;

    public ReplayService(IReplayRepo replayRepo,
        ICommentRepo commentRepo, IAuthRepo authRepo)
    {
        _replayRepo = replayRepo;
        _commentRepo = commentRepo;
        _authRepo = authRepo;
    }

    public async Task<ReplaysOnCommentResultDto> getReplaysOnComment(int commentId)
    {
        var replayList = new ReplaysOnCommentResultDto();
        var comment = await _commentRepo.GetCommentAsync(commentId);
        if (comment is null)
        {
            replayList.Message = "Invalid Comment";
            return replayList;
        }
        var replays = await _replayRepo.GetReplaysOnComment(commentId);
        if (!replays.Any())
        {
            replayList.Message = "This Comment Do not has a replays";
            return replayList;
        }
        replayList.Message = $"This List Of Replays On {comment.Id} Comment";
        replayList.Replays = replays.Select(r => new ReplayOnCommentDto
        {
            Id = r.Id,
            Replay = r.ReplayComment,
            ClientFile = r.Image,
            CreatedOn = r.CreatedOn,
            UpdatedOn = r.UpdatedOn,
            UserId = r.UserId,
        }).ToList();
        return replayList;
    }


    public async Task<ReadReplayDto> AddReplay(ReplayDto dto, string userId)
    {
        var replayR = new ReadReplayDto();
        var user = await _authRepo.FindById(userId);
        if (user is null)
        {
            replayR.Message = " Invalid user";
            return replayR;
        }
        if (dto is null)
        {
            replayR.Message = " No Replay";
            return replayR;
        }
        var comment = await _commentRepo.GetCommentAsync(dto.CommentId);
        if (comment is null)
        {
            replayR.Message = " Invalid comment To replay";
            return replayR;
        }
        var replay = new ReplayModel();
        if (dto.Replay is not null)
            replay.ReplayComment = dto.Replay;
        replay.UserId = userId;
        replay.CommentId = dto.CommentId;
        replay.CreatedOn = DateTime.Now;


        if (dto.ClientFile != null)
        {
            using var datastream = new MemoryStream();
            await dto.ClientFile.CopyToAsync(datastream);

            replay.Image = datastream.ToArray();
        }
        var replayed = await _replayRepo.addReplayAsync(replay);
        if (replayed is not null)
        {
            replayR.Message = " Replayed successfuly";
            replayR.Id = replayed.Id;
            replayR.Replay = replayed.ReplayComment;
            replayR.CreatedOn = replayed.CreatedOn;
            replayR.ClientFile = replayed.Image;
            replayR.CommentId = replayed.CommentId;
            replayR.UserId = replayed.UserId;

            comment.RepliesCount++;
            _commentRepo.updateComment(comment);

            return replayR;
        }
        replayR.Message = "Con not replay on this comment";
        return replayR;
    }


    public async Task<ReadReplayDto> EditReplay(EditReplayDto dto, string userId)
    {

        var replayR = new ReadReplayDto();
        var user = await _authRepo.FindById(userId);
        if (user is null)
        {
            replayR.Message = " Invalid user";
            return replayR;
        }
        if (dto is null)
        {
            replayR.Message = " No Replay";
            return replayR;
        }
        var comment = await _commentRepo.GetCommentAsync(dto.CommentId);
        if (comment is null)
        {
            replayR.Message = " Invalid comment To replay";
            return replayR;
        }
        var replay = await _replayRepo.GetReplay(dto.Id);
        if (replay is null)
        {
            replayR.Message = " Invalid Replay";
            return replayR;
        }
        if (dto.Replay is not null)
            replay.ReplayComment = dto.Replay;
        replay.UpdatedOn = DateTime.Now;
        replay.CommentId = dto.CommentId;
        if (dto.ClientFile != null)
        {
            using var datastream = new MemoryStream();
            await dto.ClientFile.CopyToAsync(datastream);

            replay.Image = datastream.ToArray();
        }
        _replayRepo.updateReplay(replay);

        replayR.Message = " Replay Updated successfuly";
        replayR.Id = replay.Id;
        replayR.Replay = replay.ReplayComment;
        replayR.CreatedOn = replay.CreatedOn;
        replayR.CommentId = replay.CommentId;
        replayR.UserId = replay.UserId;
        replayR.ClientFile = replay.Image;

        return replayR;
    }

    public async Task<string> DeleteReplay(int replayId, string userId)
    {
        var user = await _authRepo.FindById(userId);
        if (user is null)
            return "Invalid User";

        var replay = await _replayRepo.GetReplay(replayId);
        if (replay is null)
            return "Invalid Replay Commented";

        var comment = await _commentRepo.GetCommentAsync(replay.CommentId);
        comment.RepliesCount--;

        replay.IsDeleted = true;
        replay.DeletedOn = DateTime.Now;

        _replayRepo.updateReplay(replay);
        _commentRepo.updateComment(comment);

        return "This replay deleted...";
    }
}
