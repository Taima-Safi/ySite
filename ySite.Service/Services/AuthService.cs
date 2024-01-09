
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Repository.RepoInterfaces;
using System.Security.Claims;
using System.Security.Cryptography;
using ySite.Core.Dtos;
using ySite.Core.StaticUserRoles;
using ySite.EF.Entities;
using ySite.Service.Interfaces;

namespace ySite.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepo _authRepo;
        private readonly JwtConfiguration _jwtConfig;

        public AuthService(IAuthRepo authRepo, IOptions<JwtConfiguration> jwtConfig)
        {
            _authRepo = authRepo;
            _jwtConfig = jwtConfig.Value;
        }

        public async Task<string> RegisterUser(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
            var result =await _authRepo.InsertUser(user, dto.Password);
            return result;
        }

        public async Task<AuthDto> LoginUser(LoginDto dto)
        {
            var authDto = new AuthDto();
            var user = await _authRepo.FindByName(dto.UserName);
            if (user is null)
            {
                 authDto.msg = "You Are Not Registered";
                return authDto;
            }
            if (!await _authRepo.CheckPassword(user, dto.Password))
            {
                authDto.msg = "Password is wrong";
                return authDto;
            }
            var roles = await _authRepo.GetUserRoles(user);
            var token =  await _authRepo.GenerateTokenString(user, _jwtConfig);

            authDto.Username = user.UserName;
            authDto.Email = user.Email;
            authDto.IsAuthenticated = true;
            authDto.Roles = roles;
            authDto.Token = token;

            if(user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authDto.RefreshToken = activeRefreshToken.Token;
                authDto.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authDto.RefreshToken = refreshToken.Token;
                authDto.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _authRepo.UpdateUser(user);
            }

            //if (!string.IsNullOrEmpty(authDto.RefreshToken))
            //    SetRefreshTokenInCookie(authDto.RefreshToken, authDto.RefreshTokenExpiration);

            return authDto;
        }

        public async Task<RemoveUserDto> RemoveUser(string userId)
        {
            var removeDto = new RemoveUserDto();
            var user = await _authRepo.FindById(userId);
            if(user is null)
            {
                removeDto.Message = "Invalid User..";
                return removeDto;
            }
            if(await _authRepo.Remove(user))
            {
            removeDto.Message = "user removed ..";
            removeDto.UserName = user.UserName;
            }
            return removeDto;
        }

        public async Task<AuthDto> RefreshTokenAsync(string token)
        {
            var authDto = new AuthDto();
            var user = await _authRepo.UserfromToken(token);
            if (user is null)
            {
                authDto.IsAuthenticated = false;
                authDto.msg = "Invalid Token";
                return authDto;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshToken.IsActive)
            {
                authDto.IsAuthenticated = false;
                authDto.msg = "InActive Token";
                return authDto;
            }
            refreshToken.RevokedOn = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _authRepo.UpdateUser(user);

            var jwtToken = await _authRepo.GenerateTokenString(user, _jwtConfig);
            authDto.IsAuthenticated = true;
            authDto.Token = jwtToken;
            authDto.Username = user.UserName;
            authDto.Roles = await _authRepo.GetUserRoles(user);
            authDto.RefreshToken = newRefreshToken.Token;
            authDto.RefreshTokenExpiration = newRefreshToken.ExpiresOn; 

            return authDto;
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _authRepo.UserfromToken(token);
            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if (!refreshToken.IsActive)
                return false;
            refreshToken.RevokedOn = DateTime.UtcNow;
            await _authRepo.UpdateUser(user);
            return true;
        }

        private RefreshTokenModel GenerateRefreshToken()
        {
            var randonNum = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randonNum);
            return new RefreshTokenModel
            {
                Token = Convert.ToBase64String(randonNum),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }
        public async Task GenerateCookieAuthentication(string username)
        {
           await _authRepo.GenerateCookieAuthentication(username);
        }

        public async Task<List<Claim>> GetRoleClaims(string rolename)
        {
            return await _authRepo.GetRoleClaims(rolename);
        }

        public async Task<bool> AddUserClaim(string user, Claim claim)
        {
            if (await _authRepo.AddUserClaim(user,claim))
                return true;
            else
                return false;
        }
    }
}
