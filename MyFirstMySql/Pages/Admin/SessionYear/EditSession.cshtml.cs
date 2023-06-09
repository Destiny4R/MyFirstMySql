using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyFirstDataAccess;

namespace MyFirstMySql.Pages.Admin.SessionYear
{
    [Authorize]
    public class EditSessionModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public MyFirstModel.SessionYear session { get; set; }
        public EditSessionModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            if (id > 0)
            {
                session = dbContext.SessionYears.Find(id);
            }
        }
    public IActionResult OnPost()
    {
        //var ClaimsId = (ClaimsIdentity)User.Identity;
        //var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
        if (session.Name != null)
        {
            var classNSub = dbContext.SessionYears.Find(session.Id);
            classNSub.Name = session.Name;
                //classNSub.ApplicationUserId = claim.Value;
                dbContext.Update(classNSub);
            dbContext.SaveChanges();
            TempData["success"] = "Class Successfully Updated!";
            return RedirectToPage("Index");
        }
        return RedirectToPage();
    }
}
}
