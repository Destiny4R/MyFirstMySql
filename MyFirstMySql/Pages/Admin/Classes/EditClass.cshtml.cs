using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using MyFirstModel;
using System.Security.Claims;

namespace MyFirstMySql.Pages.Admin.Classes
{
    [Authorize]
    public class EditClassModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public ClassesInSchool school { get; set; }
        public EditClassModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            if (id > 0)
            {
                school = dbContext.ClassesInSchools.Find(id);
            }
            
        }
        public IActionResult OnPost(int id)
        {
            //var ClaimsId = (ClaimsIdentity)User.Identity;
            //var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
            if ( school.ClassName != null)
            {
                var classNSub = dbContext.ClassesInSchools.Find(school.Id);
                classNSub.ClassName = school.ClassName;
                //classNSub.ApplicationUserId = claim.Value;
                dbContext.SaveChanges();
                TempData["success"] = "Class Successfully Updated!";
                return RedirectToPage("Index");
            }
            return RedirectToPage();
        }
    }
}
