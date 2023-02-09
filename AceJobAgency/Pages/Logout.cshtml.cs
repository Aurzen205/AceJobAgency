using AceJobAgency.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AceJobAgency.Pages
{
    public class LogoutModel : PageModel
    {

        private readonly SignInManager<Membership> signInManager;
        public LogoutModel(SignInManager<Membership> signInManager)
        {
            this.signInManager = signInManager;
        }
        public void OnGet() { }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await signInManager.SignOutAsync();
            HttpContext.Session.Clear();


            return RedirectToPage("Login");
        }
        public async Task<IActionResult> OnPostDontLogoutAsync()
        {
            return RedirectToPage("Index");
        }
    }
}
