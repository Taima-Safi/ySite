using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ySite.EF.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedOn { get; set; }
    public string UserImage { get; set; }
    public GenderType Gender { get; set; }

    [NotMapped]
    public IFormFile ClientFile { get; set; }

    public string? VerificationToken { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
    public List<RefreshTokenModel>? RefreshTokens { set; get; }
    public ICollection<PostModel> Posts { get; set; }
    public ICollection<ReactionModel> Reactions { get; set; }

}

public enum GenderType
{
    Male,
    Female
}
