using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using MyFirstModel;

namespace MyFirstMySql.Pages.Admin.SessionYear
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        public IEnumerable<MyFirstModel.SessionYear> SessionYears { get; set; }
        public int data = 0;
        public IndexModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            var get = dbContext.SessionYears.ToList();
            data = get.Count();
            SessionYears = get.ToList();
        }
        public IActionResult OnPostDelete(int id)
        {
            var classNSub = dbContext.SessionYears.FirstOrDefault(k => k.Id == id);
            if (classNSub == null)
            {
                return NotFound();
            }
            dbContext.SessionYears.Remove(classNSub);
            dbContext.SaveChanges();
            TempData["success"] = "Session/Year Successfully Removed!";
            return Page();
        }
    }
}
