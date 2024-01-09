
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ySite.Core.Dtos;
using ySite.Core.StaticUserRoles;

namespace ySite.UI.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        [BindProperty]
        public LoginDto UserCredential { get; set; }

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
        }

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    var auth = await _authService.LoginUser(UserCredential);
        //    if (auth.Username is not null)
        //    {
        //      await _authService.GenerateCookieAuthentication(UserCredential.UserName);
        //        return RedirectToPage("/Index");
        //    }
        //    return Page();
        //}
    }
}
