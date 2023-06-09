using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using System.Security.Claims;

namespace MyFirstMySql.Pages.Admin.Subjects
{
    [Authorize]
    public class AddSubjectModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public MyFirstModel.Subjects Subject { get; set; }
        public AddSubjectModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            if (Subject.Name != null)
            {
                var check = dbContext.Subjects.FirstOrDefault(j => j.Name == Subject.Name);
                if (check != null)
                {
                    TempData["success"] = "Subject Already Added!";
                    return RedirectToPage("Index");
                }
                MyFirstModel.Subjects subjects = new()
                {
                    Name = Subject.Name
                };
                dbContext.Subjects.Add(subjects);
                dbContext.SaveChanges();
                TempData["success"] = "Subject Added!";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
