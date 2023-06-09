using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using MyFirstModel.ViewModel;
using MyFirstModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace MyFirstMySql.Pages.Admin.Employees
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AddSystemUserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        public string StudentRegVM = "******";
        [BindProperty]
        public ApplicationUserViewModel ApplicationUser { get; set; }
        public AddSystemUserModel(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        public void OnGet()
        {
        }
        public IActionResult OnPost(ApplicationUserViewModel ApplicationUser, IFormFile filephoto, string StudentRegVM)
        {
            long SNHolder = 0;
            string StudentReg = "";
            var getdata = dbContext.ApplicationUsers.ToList();
            if (getdata.Count() > 0)
            {
                var maxId = dbContext.ApplicationUsers.OrderByDescending(c => c.SN).FirstOrDefault();
                if (maxId == null)
                {
                    SNHolder = 0;
                }
                else
                {
                    StudentReg = $"STF" + (maxId.SN + 1).ToString("D5");
                    SNHolder = (long)(maxId.SN + 1);
                }
            }
            else
            {
                SNHolder = 0;
                StudentReg = $"STF" + (1).ToString("D5");
            }

            if (filephoto != null)
            {
                Addrole();
                var user = new ApplicationUser();

                user.UserName = StudentReg;
                user.FirstName = ApplicationUser.FirstName;
                user.OtherName = ApplicationUser.OtherName;
                user.SurName = ApplicationUser.SurName;
                user.Gender = ApplicationUser.Gender;
                user.Address = ApplicationUser.Address;
                user.DateOfBirth = ApplicationUser.DateOfBirth;
                user.State = ApplicationUser.State;
                user.PhoneNumber = ApplicationUser.PhoneNumber;
                user.LocalGov = ApplicationUser.LocalGov;
                user.PlaceOfBirth = ApplicationUser.PlaceOfBirth;
                user.Passport = GetFilePathAndCopy(filephoto, string.Empty);
                user.BloodGroup = ApplicationUser.BloodGroup;
                user.UserID = SD.IsEmplyee;
                user.SN = SNHolder;

                var result = userManager.CreateAsync(user, ApplicationUser.Password).GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, ApplicationUser.UserRole).GetAwaiter().GetResult();
                    TempData["success"] = "Employee Registration Successfully!";
                    return RedirectToPage(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }
        private void Addrole()
        {
            if (!roleManager.RoleExistsAsync(SD.Role_ExamOfficer).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Cashier)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Accountant)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_ExamOfficer)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Staff)).GetAwaiter().GetResult();
            }
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
