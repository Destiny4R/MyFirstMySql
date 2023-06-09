using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using MyFirstModel;
using System.Linq;

namespace MyFirstMySql.Pages.Admin.Classes
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        public IEnumerable<ClassesInSchool> School { get;set; } 
        public IndexModel( ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            var get = dbContext.ClassesInSchools.ToList();
            School = get.ToList();
        }
        public IActionResult OnPostDelete(int id)
        {
            var classNSub = dbContext.ClassesInSchools.FirstOrDefault(k => k.Id == id);
            if (classNSub == null)
            {
                return NotFound();
            }
            dbContext.ClassesInSchools.Remove(classNSub);
            dbContext.SaveChanges();
            TempData["success"] = "Class Successfully Removed!";
            return Page();
        }
    }
}
