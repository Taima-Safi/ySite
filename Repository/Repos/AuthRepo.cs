
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.RepoInterfaces;
//using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using ySite.Core.Dtos;
using ySite.Core.Helper;
using ySite.Core.StaticUserRoles;
using ySite.EF.DbContext;
using ySite.EF.Entities;

namespace Repository.Repos
{
    public class AuthRepo : IAuthRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IReactionRepo _reactionRepo;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContext;

        public AuthRepo(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration config,
            IHttpContextAccessor httpContext, IReactionRepo reactionRepo, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _httpContext = httpContext;
            _reactionRepo = reactionRepo;
            _context = context;
        }
        public async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
        
        public async Task UpdateUser(ApplicationUser user)
        {
            var updated = await _userManager.UpdateAsync(user);
        }

        public async Task<string> InsertUser(ApplicationUser user, string Password)
        {
            var created = await _userManager.FindByEmailAsync(user.Email);
            if (created != null)
                return "This Email already exists";
            var result = await _userManager.CreateAsync(user, Password);
            if (!result.Succeeded)
            {
                var errorString = "User Registering Faild Because : ";
                foreach (var error in result.Errors)
                    errorString += "#" + error.Description;
                return errorString;
            }
            await _userManager.AddToRoleAsync(user, UserRoles.USER);

            return "Succeeded";
        }


        public async Task<ApplicationUser> FindById(string userId)
        {
           var user = await _userManager.FindByIdAsync(userId);
            return user;
        }
        
        public async Task<ApplicationUser> FindByName(string userName)
        {
           var user = await _userManager.FindByNameAsync(userName);
            return user;
        }
         
        public async Task<ApplicationUser> FindByEmail(string email)
        {
           var user = await _userManager.FindByEmailAsync(email);
            return user;
        }
        public async Task<ApplicationUser> FindUserAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            var user = await _context.Users.FirstOrDefaultAsync(predicate);
            return user;
        }

        public async Task<bool> CheckPassword(ApplicationUser user, string password)
        {
            var found = await _userManager.CheckPasswordAsync(user, password);
             if (found)
                return true;
            return false;
        }

        public async Task<string> GenerateResetPasswordToken(ApplicationUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<string> ResetPassword(ApplicationUser user, string password)
        {
          var result =  await _userManager.ResetPasswordAsync(user, user.PasswordResetToken, password);

            if (result.Succeeded)
            {
                // Password reset successful
                return "Password reset successfully.";
            }
            else
            {
                // Password reset failed
                return "Password reset failed. Please check your token and try again.";
            }
        }

        public async Task Remove(ApplicationUser user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task<string> GenerateTokenString(ApplicationUser user, JwtConfiguration jwtConfig)
        {
            var claims = await GetClaims(user.UserName);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));
            SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtConfig.DurationInMinutes),
                issuer: jwtConfig.Issuer,
                audience: jwtConfig.Audience,
                signingCredentials: signingCred
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }

        public async Task<ApplicationUser> UserfromToken(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            return user;
        }
        
        public async Task<List<Claim>> GetClaims(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            claims.AddRange(GetClaimsSeperated(await _userManager.GetClaimsAsync(user)));
            var roles = await _userManager.GetRolesAsync(user);

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

                var identityRole = await _roleManager.FindByNameAsync(role);
                claims.AddRange(GetClaimsSeperated(await _roleManager.GetClaimsAsync(identityRole)));
            }
            return claims;
        }

        public List<Claim> GetClaimsSeperated(IList<Claim> claims)
        {
            var result = new List<Claim>();
            foreach (var claim in claims)
            {
                result.AddRange(claim.DeserializePermissions().Select(t => new Claim(claim.Type, t.ToString())));
            }
            return result;
        }

        public async Task GenerateCookieAuthentication(string username)
        {
            var claims = await GetClaims(username);

            ClaimsIdentity identity =
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal pricipal = new ClaimsPrincipal(identity);
            _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, pricipal);
        }

        public async Task<bool> AddUserClaim(string user, Claim claim)
        {
            var identityUser = await _userManager.FindByNameAsync(user);
            if (identityUser is null)
            {
                return false;
            }

            var result = await _userManager.AddClaimAsync(identityUser, claim);
            return result.Succeeded;
        }
        public async Task<List<Claim>> GetRoleClaims(string rolename)
        {
            var claims = new List<Claim>();
            var role = await _roleManager.FindByNameAsync(rolename);
            if(role is not null)
            {
                var userRoles = await _roleManager.GetClaimsAsync(role);
                foreach (var r in userRoles)
                    claims.Add(r);
                return claims;
            }
                return null;
        }
    }
}
