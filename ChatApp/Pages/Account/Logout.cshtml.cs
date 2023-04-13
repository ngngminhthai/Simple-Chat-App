using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatApp.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly ChatDbContext _context;

        public LogoutModel(ChatDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            HttpContext.SignOutAsync();
            return Redirect("/Account/Login");
        }

    }
}
