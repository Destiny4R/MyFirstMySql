using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using MyFirstModel;
using System.Data;

namespace MyFirstMySql.Pages.Account
{
        [Authorize(Roles = SD.Role_Admin)]
        [AutoValidateAntiforgeryToken]
        public class ManageUsersModel : PageModel
        {
            [BindProperty]
            public IEnumerable<ApplicationUser> ApplicationUser { get; set; }
            private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
            private readonly DateTime EndDate;
            public ManageUsersModel(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
            {
                this.userManager = userManager;
            this.dbContext = dbContext;
                EndDate = new DateTime(2132, 12, 2);
            }

            public IActionResult OnGet(int pg = 1)
            {
                ApplicationUser = dbContext.ApplicationUsers.Where(m => m.UserID == SD.IsEmplyee).ToList();
                ApplicationUser = ApplicationUser.OrderByDescending(m => m.CreateDate).ToList();
                return Page();
            }
            public async Task<IActionResult> OnPost(string mainid)
            {
                var user = await userManager.FindByIdAsync(mainid);
                if (user != null)
                {
                    var userAdmin = await userManager.GetUserAsync(User);
                    if (user.Id == userAdmin.Id)
                    {
                    await userManager.SetLockoutEnabledAsync(user, true);
                    TempData["message"] = "Admin Account Can not Be Lock";
                        OnGet(1);
                        return Page();
                    }
                    if (user.LockoutEnabled == true)
                    {
                        await userManager.SetLockoutEnabledAsync(user, false);
                        await userManager.SetLockoutEndDateAsync(user, EndDate);
                        user.LockoutEnd = EndDate;
                        await userManager.UpdateAsync(user);
                    TempData["message"] = $"User {user.UserName} Account Lock";
                        OnGet(1);
                        return Page();
                    }
                    else
                        await userManager.SetLockoutEnabledAsync(user, true);
                    await userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromMinutes(1));
                    TempData["message"] = $"User {user.UserName} Account Unlock";
                        OnGet(1);
                        return Page();
                    }
                TempData["message"] = $"User with {mainid} not found";
                    OnGet(1);
                    return Page();
                }
        }
    }
