using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repository.RepoInterfaces;
using System.Linq;
using ySite.Core.Dtos;
using ySite.Core.StaticUserRoles;
using ySite.EF.DbContext;
using ySite.EF.Entities;
using ySite.Service.Interfaces;

namespace ySite.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        //this for test
        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            return Ok(_userManager.Users.ToList());
        }
        

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm]RegisterDto dto)
        {
            return Ok(await _authService.RegisterUser(dto));
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var authModel = await _authService.LoginUser(dto);

            if (!string.IsNullOrEmpty(authModel.RefreshToken))
                SetRefreshTokenInCookie(authModel.RefreshToken, authModel.RefreshTokenExpiration);
           
            return Ok(authModel);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            var verified = await _authService.Verify(token);
            return Ok(verified);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _authService.ForgotPassword(email);
            return Ok(result);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResettPassword(ResetPasswordDto dto)
        {
            var result = await _authService.ResettPassword(dto);
            return Ok(result);
        }

        [HttpDelete("DeleteUser")]
        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            var userId = GetUserId();
            var dto = await _authService.RemoveUser(userId, Id);
            return Ok(dto);
        }

        [HttpPost("GetClaims")]
        public async Task<IActionResult> GetClaims([FromBody]string rolename)
        {
            var result = await _authService.GetRoleClaims(rolename);
            if(result is null)
                return BadRequest("No Result");

            return Ok(await _authService.GetRoleClaims(rolename));
        }
         
        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authService.RefreshTokenAsync(refreshToken);
            if (!result.IsAuthenticated)
                return BadRequest(result);
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }
        
         
        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeDto dto)
        {
            var token = dto.token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is Required!");

            var result = await _authService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is Invalid");

            return Ok("Token Revoked ..");
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime()
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
