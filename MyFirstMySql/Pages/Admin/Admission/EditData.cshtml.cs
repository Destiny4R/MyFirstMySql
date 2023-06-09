using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using MyFirstModel;

namespace MyFirstMySql.Pages.Admin.Admission
{
    [Authorize]
    public class EditDataModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }
        public EditDataModel(ApplicationDbContext dbContext,
                            UserManager<ApplicationUser> userManager,
                            IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> OnGet(string id)
        {
            ApplicationUser = await userManager.FindByIdAsync(id);
            if (ApplicationUser == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPost(ApplicationUser ApplicationUser, IFormFile filephoto)
        {
            //if (ModelState.IsValid)
            //{
            var DataFromDB = await userManager.FindByIdAsync(ApplicationUser.Id);
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
                DataFromDB.BloodGroup = ApplicationUser.BloodGroup;

                var result = await userManager.UpdateAsync(DataFromDB);
                if (result.Succeeded)
                {
                    TempData["success"] = "Student Successfully Updated!";
                    return RedirectToPage("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            //}
            TempData["error"] = "Student updating failed!";
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
