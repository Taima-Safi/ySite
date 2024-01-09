using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.Dtos;

namespace ySite.Service.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDto> LoginUser(LoginDto dto);
        Task<string> RegisterUser(RegisterDto dto);
        Task<bool> AddUserClaim(string user, Claim claim);
        Task GenerateCookieAuthentication(string username);
        Task<AuthDto> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<List<Claim>> GetRoleClaims(string rolename);
        Task<RemoveUserDto> RemoveUser(string userId);
    }
}
