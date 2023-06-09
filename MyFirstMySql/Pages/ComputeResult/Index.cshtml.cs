using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFirstDataAccess;

namespace MyFirstMySql.Pages.ComputeResult
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public IEnumerable<SelectListItem> Subject { get; set; }
        [BindProperty]
        public string session { get; set; }
        [BindProperty]
        public string classes { get; set; }
        [BindProperty]
        public string subclass { get; set; }
        [BindProperty]
        public string term { get; set; }
        [BindProperty]
        public string StudentRegNo { get; set; }
        public string Fullname = string.Empty;
        public IndexModel(ApplicationDbContext dbContext)
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
            }
            else
            {
                TempData["error"] = "Please Set Class you are working With";
            }
        }
    }
}
