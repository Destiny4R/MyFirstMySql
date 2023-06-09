using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using MyFirstModel;

namespace MyFirstMySql.Pages.Admin.SubClass
{
    [Authorize]
    public class AddSubClassModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public SubClasses subClasses { get;set; }
        public AddSubClassModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost() 
        {
            if(subClasses.ClassName!=null)
            {
                SubClasses sub = new()
                {
                    ClassName = subClasses.ClassName
                };
                dbContext.SubClasses.Add(sub);
                int result = dbContext.SaveChanges();
                if (result > 0)
                {
                    TempData["success"] = "Sub-Class Successfully Added!";
                    return RedirectToPage(nameof(Index));
                }
            }
            return Page();
        }
    }
}
