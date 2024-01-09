using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ySite.Core.Dtos.Post;

namespace ySite.UI.Pages.Post
{
    public class Post : PageModel
    {
        private readonly IPostService _postServoice;

        [BindProperty]
        public Post UserCredential { get; set; }

        public Post(IPostService postServoice)
        {
            _postServoice = postServoice;
        }

        public void OnGet()
        {
        }
    }
}
