using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.Dtos.Comments
{
    public class ReadCommentDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string? Comment { get; set; }
        public byte[]? ClientFile { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
