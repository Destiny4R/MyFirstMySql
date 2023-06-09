using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using MyFirstModel;

namespace MyFirstMySql.Pages.Admin.Classes
{
    [Authorize]
    public class AddClassModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public ClassesInSchool school { get;set; }
        public AddClassModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost() 
        {
            if(school.ClassName!=null)
            {
                ClassesInSchool inSchool = new()
                {
                    ClassName = school.ClassName
                };
                dbContext.ClassesInSchools.Add(inSchool);
                int result = dbContext.SaveChanges();
                if (result > 0)
                {
                    TempData["success"] = "Class Successfully Added!";
                    return RedirectToPage(nameof(Index));
                }
            }
            return Page();
        }
    }
}
