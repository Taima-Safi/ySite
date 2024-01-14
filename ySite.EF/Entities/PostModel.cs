using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ySite.EF.Entities
{
    public class PostModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public byte[]? Image { get; set; }
        public string UserId { get; set; } 
        public ApplicationUser User { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int ReactsCount { get; set; }
        public int CommentsCount { get; set; }

        public ICollection<ReactionModel> Reactions { get; set; } 
        public ICollection<CommentModel> Comments { get; set; } 
    }
}
