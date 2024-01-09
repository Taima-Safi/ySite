using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.Dtos.Reactions
{
    public class ReadReactionDto
    {
        public string Message { get; set; }
        public int Reaction { get; set; }
         public string UserId { get; set; }
        public int PostId { get; set; }

    }
}
