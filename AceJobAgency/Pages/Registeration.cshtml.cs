using AceJobAgency.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection;




namespace AceJobAgency.Pages
{
    public class RegisterationModel : PageModel
    {
        private IWebHostEnvironment _environment;

        public IFormFile? Upload { get; set; }
        private UserManager<Membership> userManager { get; }
        private SignInManager<Membership> signInManager { get; }
        private readonly RoleManager<IdentityRole> roleManager;

        [BindProperty]
        public Membership MModel { get; set; } = new();
        public RegisterationModel(UserManager<Membership> userManager,
        SignInManager<Membership> signInManager, IWebHostEnvironment environment, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _environment = environment;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");
                /*                var response = HttpContext.Request.Form["g-recaptcha-response"];
                                var captchaValid = await VerifyCaptchaResponse(response);*/
                /*
                                if (!captchaValid)
                                {
                                    ModelState.AddModelError("", "Captcha validation failed. Please try again.");
                                    return Page();
                                }
                */
                if (Upload != null)
                {
                    if (!(Upload.FileName.EndsWith(".docx") || Upload.FileName.EndsWith(".pdf")))
                    {
                        ModelState.AddModelError("Upload","Invalid file format. Only .docx and .pdf formats are allowed");
                        return Page();
                    }
                    if (Upload.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 2MB.");
                        return Page();
                    }
                    var uploadsFolder = "uploads";
                    var imageFile = Guid.NewGuid() + Path.GetExtension(
                    Upload.FileName);
                    var imagePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, imageFile);
                    Debug.WriteLine(imagePath);
                    using var fileStream = new FileStream(imagePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    MModel.ResumeURL = string.Format("/{0}/{1}", uploadsFolder, imageFile);
                }


                var user = new Membership()
                {
                    FirstName = MModel.FirstName,
                    LastName = MModel.LastName,
                    Gender = MModel.Gender,
                    NRIC = protector.Protect(MModel.NRIC),
                    Email = MModel.Email,
                    DateOfBirth = MModel.DateOfBirth,
                    ResumeURL = MModel.ResumeURL,
                    WhoamI = MModel.WhoamI,
                    UserName = MModel.Email
                };
                //Create the Admin role if NOT exist
                IdentityRole role = await roleManager.FindByIdAsync("Admin");
                if (role == null)
                {
                    IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("Admin"));
                    if (!result2.Succeeded)
                    {
                        ModelState.AddModelError("", "Create role admin failed");
                    }
                }



                var result = await userManager.CreateAsync(user, MModel.Password);
                if (result.Succeeded)
                {
                    result = await userManager.AddToRoleAsync(user, "Admin");
                    await signInManager.SignInAsync(user, false);
                    HttpContext.Session.SetString("UserId", user.Id);
                    return RedirectToPage("Login");
                }

                foreach (var error in result.Errors)
                {

                    ModelState.AddModelError("", error.Description);

                }


            }
            return Page();
        }

/*        private async Task<bool> VerifyCaptchaResponse(string response)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                var secretKey = "6Lf-zVUkAAAAAMjxZ__UzmMS7Gbbw2d9y-26cM14";
                var request = $"secret={secretKey}&response={response}";
                var content = new StringContent(request, Encoding.UTF8, "application/x-www-form-urlencoded");

                var responseMessage = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var result = await responseMessage.Content.ReadAsStringAsync();
                    var resultObject = JsonConvert.DeserializeObject<dynamic>(result);
                    if (resultObject.success)
                    {
                        return true;
                    }
                }
            }

            return false;
        }*/





        public void OnGet()
        {
        }
    }
}
