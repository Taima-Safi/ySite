using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.EF.Entities
{
    public class FriendShipModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public FStatus Status { get; set; }
        public ApplicationUser User { get; set; }
        public ApplicationUser Friend { get; set; }
    }

    public enum FStatus
    {
        Pending,
        Accepted,
        Declined
    }
}
