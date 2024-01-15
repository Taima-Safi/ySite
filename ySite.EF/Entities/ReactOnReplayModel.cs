using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.EF.Entities
{
    public class ReactOnReplayModel
    {
        public int Id { get; set; }
        public ReactionType Reaction { get; set; }
        public string UserId { get; set; }
        public int ReplayId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool IsDeleted { get; set; } = false;

        public ApplicationUser User { get; set; }
        public ReplayModel Replay { get; set; }
    }
}
