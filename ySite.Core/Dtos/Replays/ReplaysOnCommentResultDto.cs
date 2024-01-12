using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.Dtos.Replays
{
    public class ReplaysOnCommentResultDto
    {
        public string Message { get; set; }
        public List<ReplayOnCommentDto> Replays { get; set; }
    }

    public class ReplayOnCommentDto
    {
        public int Id { get; set; }
        public string? Replay { get; set; }
        public byte[]? ClientFile { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
