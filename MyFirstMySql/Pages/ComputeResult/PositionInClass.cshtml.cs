using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyFirstDataAccess;
using MyFirstModel;

namespace MyFirstMySql.Pages.ComputeResult
{
    [Authorize]
    public class PositionInClassModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        public IEnumerable<TermregistrationTable> TermReg { get; set; }
        public IEnumerable<ResultTable> ResultTablez { get; set; }
        public IEnumerable<RemarkPosition> Remark { get;set; }
        [BindProperty]
        public string session { get; set; }
        [BindProperty]
        public string classes { get; set; }
        [BindProperty]
        public string subclass { get; set; }
        [BindProperty]
        public string term { get; set; }
        public int no = 0;
        public PositionInClassModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            term = HttpContext.Session.GetString("term");
            if (term != null)
            {
                var getsession = dbContext.SessionYears.FirstOrDefault(j => j.Id == HttpContext.Session.GetInt32("session"));
                session = getsession.Name;
                var getclasses = dbContext.ClassesInSchools.FirstOrDefault(j => j.Id == HttpContext.Session.GetInt32("classes"));
                classes = getclasses.ClassName;
                var getsubclass = dbContext.SubClasses.FirstOrDefault(j => j.Id == HttpContext.Session.GetInt32("subclass"));
                subclass = getsubclass.ClassName;

                TermReg = dbContext.Termregistrations.Include(k => k.ApplicationUser).Include(k => k.SessionYear).Include(k => k.ClassesInSchool).Include(k => k.SubClasses).Where(h => h.ClassesInSchoolId == HttpContext.Session.GetInt32("classes") && h.SubClassId == HttpContext.Session.GetInt32("subclass") && h.Term == HttpContext.Session.GetString("term") && h.SessionYearId == HttpContext.Session.GetInt32("session")).ToList();

                no = TermReg.Count();
                ResultTablez = dbContext.ResultTables.ToList();
                Remark = dbContext.RemarkPositions.ToList();
            }
            else
            {
                TempData["error"] = "Please Set Class you are working With";
            }
        }
    }
}
