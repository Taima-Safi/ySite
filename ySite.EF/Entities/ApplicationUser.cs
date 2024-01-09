using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ySite.EF.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName{ get; set; }
        public string LastName{ get; set; }
        public bool IsDeleted { get; set; } = false;

        public List<RefreshTokenModel>? RefreshTokens { set; get; }
        public ICollection<PostModel> Posts { get; set; }
        public ICollection<ReactionModel> Reactions { get; set; }

    }
}
