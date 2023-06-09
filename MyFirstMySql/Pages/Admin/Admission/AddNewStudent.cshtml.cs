using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using MyFirstModel;
using MyFirstModel.ViewModel;
using System.Security.Claims;

namespace MyFirstMySql.Pages.Admin.Admission
{
    [Authorize]
    public class AddNewStudentModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public ApplicationUserViewModel ApplicationUser { get; set; }
        public AddNewStudentModel( ApplicationDbContext dbContext,
                                   UserManager<ApplicationUser> userManager,
                                   IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost(ApplicationUserViewModel ApplicationUser, IFormFile filephoto)
        {
            long SNHolder = 0;
            string StudentReg = "";
            var getdata = dbContext.ApplicationUsers.ToList();
            if (getdata.Count() > 0)
            {
                var maxId = dbContext.ApplicationUsers.OrderByDescending(c => c.SN).FirstOrDefault();
                if (maxId == null)
                {
                    
                }
                else
                {
                    StudentReg = $"STD" + (maxId.SN + 1).ToString("D10");
                    SNHolder = (long)(maxId.SN + 1);
                }
            }
            else
            {
                SNHolder = 0;
                StudentReg = $"STD" + (1).ToString("D10");
            }
            
            
            if (filephoto != null)
            {
                var ClaimsId = (ClaimsIdentity)User.Identity;
                var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
                var user = new ApplicationUser();

                user.UserName = StudentReg;
                user.StudentRegNo = StudentReg;
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
                user.UserID = SD.IsStudent;
                user.SN = SNHolder;
                user.RegisterBy = claim.Value;

                var result = await userManager.CreateAsync(user, ApplicationUser.Password);
                if (result.Succeeded)
                {
                    TempData["success"] = "Student Registration Successfully!";
                    return RedirectToPage(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
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
