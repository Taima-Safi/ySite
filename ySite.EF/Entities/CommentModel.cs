using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.EF.Entities
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public byte[]? Image { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; } = false;
        //Add ReactsCount prop
        public int RepliesCount { get; set; }
        public ApplicationUser User { get; set; }
        public PostModel Post { get; set; }

    }
}
