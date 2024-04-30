namespace ySite.EF.Entities
{
    public class ReplayModel
    {
        public int Id { get; set; }
        public string? ReplayComment { get; set; }
        public byte[]? Image { get; set; }
        public string UserId { get; set; } //How Replay
        public int CommentId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; } = false;
        //Add LikesCount prop
        public ApplicationUser User { get; set; }
        public CommentModel Comment { get; set; }

        public ICollection<ReactOnReplayModel> ReactOnReplays { get; set; }

    }
}
