using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;

namespace MyFirstMySql.Pages.Admin.SessionYear
{
    [Authorize]
    public class AddSessionModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public MyFirstModel.SessionYear session { get; set; }
        public AddSessionModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (session.Name != null)
            {
                MyFirstModel.SessionYear sessionYear = new()
                {
                    Name = session.Name
                };
                dbContext.SessionYears.Add(sessionYear);
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
