using ChatApp.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatApp.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ChatDbContext _context;

        [BindProperty]
        public User User { get; set; }

        public RegisterModel(ChatDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            HttpContext.SignOutAsync();
            return Redirect("/Account/Login");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }

}
