using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using MyFirstModel;
using System.Linq;

namespace MyFirstMySql.Pages.Admin.SubClass
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        public int dataz = 0;
        public IEnumerable<SubClasses> subClasses { get;set; } 
        public IndexModel( ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            var get = dbContext.SubClasses.ToList();
            dataz = get.Count();
            subClasses = get.ToList();
        }
        public IActionResult OnPostDelete(int id)
        {
            var classNSub = dbContext.SubClasses.FirstOrDefault(k => k.Id == id);
            if (classNSub == null)
            {
                return NotFound();
            }
            dbContext.SubClasses.Remove(classNSub);
            dbContext.SaveChanges();
            TempData["success"] = "Sub-Class Successfully Removed!";
            return Page();
        }
    }
}
