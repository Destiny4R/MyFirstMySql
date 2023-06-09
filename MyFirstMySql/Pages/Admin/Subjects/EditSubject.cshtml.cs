using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;
using System.Security.Claims;

namespace MyFirstMySql.Pages.Admin.Subjects
{
    [Authorize]
    public class EditSubjectModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public MyFirstModel.Subjects Subject { get; set; }
        public EditSubjectModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            Subject = dbContext.Subjects.FirstOrDefault(k => k.Id == id);
            if(Subject == null)
            {
                return RedirectToPage("Index");
            }
            return Page();
        }
        public IActionResult OnPost(int id)
        {
            if (Subject.Name != "")
            {
                var subject = dbContext.Subjects.FirstOrDefault(k => k.Name == Subject.Name);
                if (subject != null)
                {
                    TempData["success"] = "Subject Already Added!";
                    return RedirectToPage("Index");
                }
                var check = dbContext.Subjects.FirstOrDefault(k => k.Id == id);
                check.Name = Subject.Name;
                dbContext.Subjects.Update(check);
                dbContext.SaveChanges();
                TempData["success"] = "Subject Successfully Updated!";
                return RedirectToPage("Index");
            }
            return RedirectToPage();
        }
    }
}
