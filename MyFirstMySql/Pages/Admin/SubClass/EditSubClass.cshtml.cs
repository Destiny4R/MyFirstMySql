using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using MyFirstModel;
using System.Security.Claims;

namespace MyFirstMySql.Pages.Admin.SubClass
{
    [Authorize]
    public class EditSubClassModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public SubClasses subClasses { get; set; }
        public EditSubClassModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            if (id > 0)
            {
                subClasses = dbContext.SubClasses.Find(id);
            }
            
        }
        public IActionResult OnPost(int id)
        {
            //var ClaimsId = (ClaimsIdentity)User.Identity;
            //var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
            if ( subClasses.ClassName != null)
            {
                var classNSub = dbContext.SubClasses.Find(subClasses.Id);
                classNSub.ClassName = subClasses.ClassName;
                //classNSub.ApplicationUserId = claim.Value;
                dbContext.SaveChanges();
                TempData["success"] = "Sub-Class Successfully Updated!";
                return RedirectToPage("Index");
            }
            return RedirectToPage();
        }
    }
}
