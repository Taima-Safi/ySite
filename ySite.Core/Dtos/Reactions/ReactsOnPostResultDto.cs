using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.Dtos.Post;

namespace ySite.Core.Dtos.Reactions
{
    public class ReactsOnPostResultDto
    {
        public string Message { get; set; }
        public List<ReactsOnPostDto> Reactions { get; set; }
    }
    public class ReactsOnPostDto
    {
        public int Reaction { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
