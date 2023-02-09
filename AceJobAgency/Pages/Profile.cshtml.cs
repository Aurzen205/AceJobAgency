using AceJobAgency.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.DataProtection;


namespace AceJobAgency.Pages
{
    public class ProfileModel : PageModel
    {
        private UserManager<Membership> userManager { get; }
        private SignInManager<Membership> signInManager { get; }

        public Membership MModel { get; set; } = new();


        public ProfileModel(UserManager<Membership> userManager, SignInManager<Membership> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
            var protector = dataProtectionProvider.CreateProtector("MySecretKey");
            if (HttpContext.Session.GetString("UserId")  == null)
            {
                return RedirectToPage("/Login");

            }
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return Redirect("/Login");
            }
            else
            {
                MModel.FirstName = user.FirstName;
                MModel.LastName = user.LastName;
                MModel.NRIC = protector.Unprotect(user.NRIC);
                MModel.WhoamI = user.WhoamI;





                return Page();
            }




        }

    }
}
