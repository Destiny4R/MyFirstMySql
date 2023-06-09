using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using MyFirstModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyFirstMySql.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        [BindProperty]
        [System.ComponentModel.DataAnnotations.Required]
        public string Username { get; set; }
        [BindProperty]
        [System.ComponentModel.DataAnnotations.Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
        [BindProperty]
        public string? ReturnUrl { get; set; }
        public LoginModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        public void OnGet(string? returnUrl)
        {
            ReturnUrl = returnUrl;
        }
        public async Task<IActionResult> OnPost()
        {
            bool lockme = true;
            if (Username != null && Password != null)
            {
                var result = await signInManager.PasswordSignInAsync(Username, Password, RememberMe, true);
                if (result.Succeeded)
                {
                    var getinfo = await userManager.FindByNameAsync(Username);
                    if (getinfo.LockoutEnabled != lockme)
                    {
                        await signInManager.SignOutAsync();
                        ModelState.AddModelError("", $"User Account {getinfo.SurName + ' ' + getinfo.OtherName + ' ' + getinfo.FirstName} Has Been Disabled, Plaese Contact Your System Administrator For More Information");
                        TempData["error"] = $"User Account {getinfo.SurName + ' ' + getinfo.OtherName + ' ' + getinfo.FirstName} Has Been Disabled, Plaese Contact Your System Administrator For More Information";
                        return Page();
                    }
                    if (getinfo.UserID == SD.IsStudent)
                    {
                        return Redirect("~/Portal/Index");
                    }
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        if (ReturnUrl == "/")
                        {
                            return RedirectToPage("/Index");
                        }
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToPage("/Index");
                    }
                }
                ModelState.AddModelError("", "Wrong Username or Password");
            }
            return Page();
        }
    }
}
