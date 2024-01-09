using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.Dtos.Post
{
    public class UserPostsResultDto
    {
        public string Message { get; set; }
        public List<UserPostsDto> Posts { get; set; }
    }
}
