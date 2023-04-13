using DemoSignalR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ChatDbContext _context;
        private readonly IHubContext<signalrServer> _signalrHub;


        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public LoginModel(ChatDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == Username && u.PassWord == Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                await HttpContext.SignOutAsync();
                return Page();
            }
            else
            {
                //Add claims for logged in user
                var claims = new List<Claim>
                {
                    new Claim("Id", (user.Id).ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                var identity = new ClaimsIdentity(claims, "custom");
                var principal = new ClaimsPrincipal(identity);



                await HttpContext.SignInAsync(principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(7)
                });
            }

            // TODO: Store the user's ID in a session variable or cookie.

            return RedirectToPage("/Index");
        }
    }

}
