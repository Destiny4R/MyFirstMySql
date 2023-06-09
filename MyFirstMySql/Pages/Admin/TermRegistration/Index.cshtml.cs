using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyFirstDataAccess;

namespace MyFirstMySql.Pages.Admin.TermRegistration
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public IndexModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost(int id)
        {
            var student = dbContext.Termregistrations.FirstOrDefault(k => k.Id == id);
            if (student == null)
            {
                TempData["error"] = "Error while delete";
                return Page();
            }
            dbContext.Remove(student);
            dbContext.SaveChanges();

            TempData["success"] = "Student Successfully Removed From the Term";
            return Page();
        }
    }
}
