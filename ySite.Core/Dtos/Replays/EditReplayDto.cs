using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.Dtos.Replays
{
    public class EditReplayDto
    {
        public int Id { get; set; }
        public string? Replay { get; set; }
        public IFormFile? ClientFile { get; set; }
        //public string UserId { get; set; }
        public int CommentId { get; set; }
    }
}
