using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFirstDataAccess;
using MyFirstModel;

namespace MyFirstMySql.Pages.ComputeResult
{
    [Authorize]
    public class PositionInSubjectModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public int subjectid { get; set; }
        public IEnumerable<SelectListItem> Subject { get; set; }
        public IEnumerable<TermregistrationTable> TermReg { get; set; }
        public IEnumerable<ResultTable> ResultTablez { get; set; }
        [BindProperty]
        public string session { get; set; }
        [BindProperty]
        public string classes { get; set; }
        [BindProperty]
        public string subclass { get; set; }
        [BindProperty]
        public string term { get; set; }
        public PositionInSubjectModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public int no = 0;
        public void OnGet()
        {
            GetSubjects();
            term = HttpContext.Session.GetString("term");
            if (term != null)
            {
                var getsession = dbContext.SessionYears.FirstOrDefault(j => j.Id == HttpContext.Session.GetInt32("session"));
                session = getsession.Name;
                var getclasses = dbContext.ClassesInSchools.FirstOrDefault(j => j.Id == HttpContext.Session.GetInt32("classes"));
                classes = getclasses.ClassName;
                var getsubclass = dbContext.SubClasses.FirstOrDefault(j => j.Id == HttpContext.Session.GetInt32("subclass"));
                subclass = getsubclass.ClassName;
            }
            else
            {
                TempData["error"] = "Please Set Class you are working With";
            }
        }
        public IActionResult OnPost()
        {
            ResultTablez = dbContext.ResultTables.Include(j=>j.TermregistrationTable).ThenInclude(k => k.ApplicationUser).Include(k => k.SessionYear).Include(k => k.ClassesInSchool).Include(k => k.SubClasses).Include(u=>u.Subjects).Where(h => h.ClassesId == HttpContext.Session.GetInt32("classes") && h.SubClassId == HttpContext.Session.GetInt32("subclass") && h.Term == HttpContext.Session.GetString("term") && h.SessionYearId == HttpContext.Session.GetInt32("session") && h.Subjects.Id==subjectid).ToList();
            ResultTablez = ResultTablez.OrderByDescending(k => k.Total);
            no = ResultTablez.Count();
            GetSubjects();
            return Page();
        }
        public void GetSubjects()
        {
            Subject = dbContext.Subjects.Select(k => new SelectListItem
            {
                Text = k.Name,
                Value = k.Id.ToString()
            });
        }
    }
}
