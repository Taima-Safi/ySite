
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ySite.UI.Pages.Account
{
   // [Authorize]
    public class AddUserModel : PageModel
    {
        private IAuthService _authService;

        [BindProperty]
        public RegisterDto UserCredential { get; set; }

        public AddUserModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                return BadRequest();
            }
            if (await _authService.RegisterUser(UserCredential) is not null)
            {
                return RedirectToPage("/Account/Login");
            }
            return BadRequest();
        }
    }
}
