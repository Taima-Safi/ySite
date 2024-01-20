using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Repository.RepoInterfaces;
using System.Security.Claims;
using System.Security.Cryptography;
using ySite.Core.Dtos;
using ySite.Core.StaticFiles;
using ySite.Core.StaticUserRoles;
using ySite.EF.Entities;
using ySite.Service.Interfaces;

namespace ySite.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepo _authRepo;
        private readonly IPostRepo _postRepo;
        private readonly IReactionRepo _reactionRepo;
        private readonly ICommentRepo _commentRepo;
        private readonly JwtConfiguration _jwtConfig;
        private readonly IHostingEnvironment _host;
        private readonly string _imagepath;

        public AuthService(IAuthRepo authRepo,
            IOptions<JwtConfiguration> jwtConfig,
            IHostingEnvironment host,
            IReactionRepo reactionRepo,
            ICommentRepo commentRepo,
            IPostRepo postRepo)
        {
            _authRepo = authRepo;
            _jwtConfig = jwtConfig.Value;
            _host = host;
            _imagepath = $"{_host.WebRootPath}{FilesSettings.ImagesPath}";
            _reactionRepo = reactionRepo;
            _commentRepo = commentRepo;
            _postRepo = postRepo;
        }

        public async Task<AuthDto> RegisterUser(RegisterDto dto)
        {
            var authDto = new AuthDto();
            if (dto == null)
            {
                authDto.msg = "Invalid Value";
                return authDto;
            }
            var user = new ApplicationUser();

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.UserName = dto.UserName;
            user.Email = dto.Email;
            user.Gender = (GenderType)dto.Gender;
            user.PhoneNumber = dto.PhoneNumber;
            user.CreatedOn = DateTime.Now;
            user.VerificationToken = CreateRandomToken();

            string fileName = string.Empty;
            if(dto.ClientFile is null)
            {
                if ((GenderType)dto.Gender == GenderType.Female)
                {
                    var femaleImage = FilesSettings.DefaultFemaleImagePath;
                    user.UserImage = Path.GetFileName(femaleImage);
                }
                else
                {
                    var maleImage = FilesSettings.DefaultUserImagePath;
                    user.UserImage = Path.GetFileName(maleImage);
                }
            }
            else
            {
                var result = FilesSettings.UserImageAllowUplaod(dto.ClientFile);
                if (result.IsValid)
                {
                    string myUpload = Path.Combine(_imagepath, "defaultUserImage");
                    fileName = dto.ClientFile.FileName;
                    string fullPath = Path.Combine(myUpload, fileName);

                    dto.ClientFile.CopyTo(new FileStream(fullPath, FileMode.Create));
                    user.UserImage = fileName;
                }
                else
                {
                    authDto.msg = result.Message;
                    return authDto;
                }
            }

           var registered =await _authRepo.InsertUser(user, dto.Password);

            var roles = await _authRepo.GetUserRoles(user);
            var token = await _authRepo.GenerateTokenString(user, _jwtConfig);
            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens?.Add(refreshToken);
            await _authRepo.UpdateUser(user);

            authDto.msg = registered;
            authDto.Username = user.UserName;
            authDto.Email = user.Email;
            authDto.Gender = (int)user.Gender;
            authDto.IsAuthenticated = true;
            authDto.Roles = roles;
            authDto.Token = token;
            authDto.RefreshToken = refreshToken.Token;
            authDto.RefreshTokenExpiration = refreshToken.ExpiresOn;

            return authDto;
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
            if (user.VerifiedAt == null)
            {
                authDto.msg = "Not verified!";
                return authDto;
            }
            var roles = await _authRepo.GetUserRoles(user);
            var token =  await _authRepo.GenerateTokenString(user, _jwtConfig);

            authDto.Username = user.UserName;
            authDto.Email = user.Email;
            authDto.Gender = (int)user.Gender;
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

            return authDto;
        }


        public async Task<string> Verify(string token)
        {
            var user = await _authRepo.FindUserAsync(u => u.VerificationToken == token);
            if (user == null)
                return "Invalid token";

            user.VerifiedAt = DateTime.Now;
            await _authRepo.UpdateUser(user);
            return "Verified successfuly";
        }

        public async Task<string> ForgotPassword(string email)
        {
            var user = await _authRepo.FindByEmail(email);
            if (user == null)
            {
                return "User not found.";
            }

            user.PasswordResetToken = await _authRepo.GenerateResetPasswordToken(user);
            user.ResetTokenExpires = DateTime.Now.AddDays(1);

            await _authRepo.UpdateUser(user);

            return "You may now reset your password.";
        }

        public async Task<string> ResettPassword(ResetPasswordDto dto)
        {
           var user =  await _authRepo.FindByEmail(dto.Email);
            if (user == null|| user.PasswordResetToken == null || user.ResetTokenExpires < DateTime.Now)
            {
                return "Invalid Token.";
            }
            var result = await _authRepo.ResetPassword(user, dto.NewPassword);
    
            return result;
        }

        public async Task<RemoveUserDto> RemoveUser(string authId, string userId)
        {
            var removeDto = new RemoveUserDto();
            var user = await _authRepo.FindById(userId);
            var authedUser = await _authRepo.FindById(authId);
            var authedRole = await _authRepo.GetUserRoles(authedUser);
            if(user is null)
            {
                removeDto.Message = "Invalid User..";
                return removeDto;
            }

            if(user.Id != authId && !authedRole.Contains(UserRoles.OWNER))
            {
                removeDto.Message = "You Do not have permissions to Remove this user";
                return removeDto;
            }

            var reactions = await _reactionRepo.GetReactionsForUser(user.Id);
            var comments = await _commentRepo.GetCommentsAsync(user);
            var posts = await _postRepo.GetPostsAsync(user);

            user.IsDeleted = true;
            foreach (var reaction in reactions)
            {
                 reaction.IsDeleted = true;
                 _reactionRepo.updateReaction(reaction);
            }
            
            foreach (var comment in comments)
            {
                 comment.IsDeleted = true;
                _commentRepo.updateComment(comment);
            }
            
            foreach (var post in posts)
            {
                 post.IsDeleted = true;
                _postRepo.updatePost(post);
            }

            await _authRepo.Remove(user);

            removeDto.Message = "user removed ..";
            removeDto.UserName = user.UserName;

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
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
