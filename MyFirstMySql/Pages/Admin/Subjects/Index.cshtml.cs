using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstDataAccess;

namespace MyFirstMySql.Pages.Admin.Subjects
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public IndexModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public int no;
        public IEnumerable<MyFirstModel.Subjects> Subject { get; set; }
        public void OnGet()
        {
            Subject = dbContext.Subjects.ToList();
            no = Subject.Count();
        }
        public IActionResult OnPost(int id)
        {
            var subject = dbContext.Subjects.FirstOrDefault(k => k.Id == id);
            if (subject == null)
            {
                return NotFound();
            }
            //dbContext.Subjects.Remove(subject);
            //dbContext.SaveChanges();
            TempData["success"] = "Subject Remove!";
            return Page();
        }
    }
}
