using AceJobAgency.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AceJobAgency.Pages
{
    public class LoginModel : PageModel
    {

        [BindProperty]
        public Login LModel { get; set; }

        private readonly SignInManager<Membership> signInManager;
        public LoginModel(SignInManager<Membership> signInManager)
        {
            this.signInManager = signInManager;
        }
        public void OnGet()
        {
        }
        protected void LogoutMe(object sender, EventArgs e)
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,LModel.RememberMe, lockoutOnFailure:true);
                if (identityResult.IsLockedOut)
                {
                    
                    ModelState.AddModelError("", "Account locked out due to too many failed login attempts. Please try again later.");
                    return Page();

                }
                else
                {
                    if (identityResult.Succeeded)
                    {
                        var user = await signInManager.UserManager.GetUserAsync(User);
                        HttpContext.Session.SetString("UserId", user.Id);
                        return RedirectToPage("Index");
                    }
                    ModelState.AddModelError("", "Username or Password incorrect");

                }
                        }
            return Page();
        }


    }
}
