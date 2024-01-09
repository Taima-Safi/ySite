using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.Dtos.Comments
{
    public class UserCommentsResultDto
    {
        public string Message { get; set; }
        public List<ReadCommentDto> Comments { get; set; }
    }
}
