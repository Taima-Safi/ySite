using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.EF.Entities
{
    public class ReactionModel
    {
        public int Id { get; set; }
        public ReactionType Reaction{ get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; } = false;
       
        public ApplicationUser User { get; set; }
        public PostModel Post { get; set; }
    }

    public enum ReactionType
    {
        Like,
        Love,
        Sad,
        Angry,
    }
}
