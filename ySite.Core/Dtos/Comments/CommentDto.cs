using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.Dtos.Comments
{
    public class CommentDto
    {
        public string? Comment { get; set; }
        public IFormFile? ClientFile { get; set; }
        //public string UserId { get; set; }
        public int PostId { get; set; }
        //public DateTime CreatedOn { get; set; }

    }
}
