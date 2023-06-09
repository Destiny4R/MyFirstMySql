using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyFirstDataAccess;
using MyFirstModel;

namespace MyFirstMySql.Pages.ComputeResult
{
    [Authorize]
    public class ViewResultByStudentModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        public string StudentRegNo, fullname;
        public IEnumerable<ResultTable> Results { get; set; }
        public ViewResultByStudentModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult OnGet(int id)
        {
            if (HttpContext.Session.GetString("term") != null)
            {
                Results = dbContext.ResultTables.Include(m => m.TermregistrationTable).ThenInclude(k => k.ApplicationUser).Include(k => k.SubClasses).Include(k => k.Subjects).Include(n => n.ClassesInSchool).Include(s => s.SessionYear).Where(h => h.ClassesId == HttpContext.Session.GetInt32("classes") && h.SubClassId == HttpContext.Session.GetInt32("subclass") && h.Term == HttpContext.Session.GetString("term") && h.SessionYearId == HttpContext.Session.GetInt32("session") && h.TermRegId == id).ToList();
                if (Results == null)
                {
                    TempData["success"] = "Please Add Result First";
                    return RedirectToPage("Index");
                }
                var info = Results.FirstOrDefault();
                StudentRegNo = info.TermregistrationTable.ApplicationUser.StudentRegNo;
                fullname = info.TermregistrationTable.ApplicationUser.SurName + " " + info.TermregistrationTable.ApplicationUser.OtherName + " " + info.TermregistrationTable.ApplicationUser.FirstName;
                return Page();
            }
            return RedirectToPage("Index");
        }
        public IActionResult OnPost(int id)
        {
            if (id > 0)
            {
                var getresult = dbContext.ResultTables.Find(id);
                if (getresult != null)
                {
                    dbContext.ResultTables.Remove(getresult);
                    dbContext.SaveChanges();
                    TempData["success"] = "Result Successfully Removed";
                    return Page();
                }
            }
            TempData["success"] = "Something Happened Result Not Remove";
            return Page();
        }
    }
}
