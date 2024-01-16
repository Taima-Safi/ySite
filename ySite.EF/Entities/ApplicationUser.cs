using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ySite.EF.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName{ get; set; }
        public string LastName{ get; set; }
        public bool IsDeleted { get; set; } = false;
        public string UserImage { get; set; }

        [NotMapped]
        public IFormFile ClientFile { get; set; }

        public List<RefreshTokenModel>? RefreshTokens { set; get; }
        public ICollection<PostModel> Posts { get; set; }
        public ICollection<ReactionModel> Reactions { get; set; }

    }
}
