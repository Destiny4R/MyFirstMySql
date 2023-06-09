using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using MyFirstModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyFirstMySql.Pages.Admin.Employees
{
    [Authorize(Roles = SD.Role_Admin)]
    public class EditSystemUserModel : PageModel
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        //[BindProperty]
        public ApplicationUser ApplicationUser { get; set; }
        [BindProperty]
        [Display(Name = "User Role")]
        public string RoleName { get; set; }
        public EditSystemUserModel(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult OnGet(string Id)
        {
            ApplicationUser = userManager.FindByIdAsync(Id).GetAwaiter().GetResult();
            var dBRole = userManager.GetRolesAsync(ApplicationUser).Result.FirstOrDefault();
            RoleName = dBRole;
            if (ApplicationUser == null)
            {
                return NotFound();
            }
            return Page();
        }
        public IActionResult OnPost(ApplicationUser ApplicationUser, IFormFile filephoto)
        {
            var DataFromDB = userManager.FindByIdAsync(ApplicationUser.Id).GetAwaiter().GetResult();
            if (DataFromDB != null)
            {
                DataFromDB.FirstName = ApplicationUser.FirstName;
                DataFromDB.OtherName = ApplicationUser.OtherName;
                DataFromDB.SurName = ApplicationUser.SurName;
                DataFromDB.Gender = ApplicationUser.Gender;
                DataFromDB.Address = ApplicationUser.Address;
                DataFromDB.DateOfBirth = ApplicationUser.DateOfBirth;
                DataFromDB.State = ApplicationUser.State;
                DataFromDB.PhoneNumber = ApplicationUser.PhoneNumber;
                DataFromDB.LocalGov = ApplicationUser.LocalGov;
                DataFromDB.PlaceOfBirth = ApplicationUser.PlaceOfBirth;
                if (filephoto != null)
                {
                    DataFromDB.Passport = GetFilePathAndCopy(filephoto, ApplicationUser.Passport);
                }
                else
                {
                    DataFromDB.Passport = ApplicationUser.Passport;
                }
                DataFromDB.BloodGroup = ApplicationUser.BloodGroup;

                var result = userManager.UpdateAsync(DataFromDB).GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    var dBRole = userManager.GetRolesAsync(DataFromDB).Result.FirstOrDefault();
                    if (dBRole != null)
                    {
                        var removeRole = userManager.RemoveFromRoleAsync(DataFromDB, dBRole).GetAwaiter().GetResult();
                        if (removeRole.Succeeded)
                            userManager.AddToRoleAsync(DataFromDB, RoleName).GetAwaiter().GetResult();
                    }
                    TempData["success"] = "User Successfully updated!";
                    return RedirectToPage("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            TempData["error"] = "User updating failed!";
            return RedirectToPage("Index");
        }
        public string GetFilePathAndCopy(IFormFile file, string imagePath)
        {
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var upload = Path.Combine(webHostEnvironment.WebRootPath, @"assets\images\Passports\");
                var extention = Path.GetExtension(file.FileName);
                if (imagePath != null)
                {
                    string deleteOldImage = Path.Combine(webHostEnvironment.WebRootPath, imagePath);
                    if (System.IO.File.Exists(deleteOldImage))
                    {
                        System.IO.File.Delete(deleteOldImage);
                    }
                }
                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return @"/assets\images\Passports\" + fileName + extention;
            }
            return string.Empty;
        }
    }
}
