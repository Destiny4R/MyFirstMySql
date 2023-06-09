using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFirstDataAccess;
using MyFirstModel;
using MyFirstModel.ViewModel;

namespace MyFirstMySql.Pages.ComputeResult
{
    [Authorize]
    public class AddResultModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public IEnumerable<SelectListItem> Subject {  get; set; }
        [BindProperty]
        public string session { get; set; }
        [BindProperty]
        public string classes { get; set; }
        [BindProperty]
        public string subclass { get; set; }
        [BindProperty]
        public string term { get; set; }
        public IEnumerable<ResultViewModel> ResultTable {  get; set; }
        [BindProperty]
        public TermregistrationTable termreg {  get; set; }
        [BindProperty]
        public string StudentRegNo { get; set; }
        public string Fullname=string.Empty;
        public AddResultModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult OnGet( IEnumerable<ResultViewModel> results)
        {
            ResultTable = results;
            if (HttpContext.Session.GetInt32("session") == null)
            {
                TempData["error"] = "Please go to setting and set up the class your working with before anding results";
                return Page();
            }
            term = HttpContext.Session.GetString("term");
            var getsession = dbContext.SessionYears.FirstOrDefault(j=>j.Id== HttpContext.Session.GetInt32("session"));
            session = getsession.Name;
            var getclasses = dbContext.ClassesInSchools.FirstOrDefault(j => j.Id == HttpContext.Session.GetInt32("classes"));
            classes = getclasses.ClassName;
            var getsubclass = dbContext.SubClasses.FirstOrDefault(j => j.Id == HttpContext.Session.GetInt32("subclass"));
            subclass = getsubclass.ClassName;
            GetSubjects();
            return Page();
        }
        public IActionResult OnPostSearch(IEnumerable<ResultViewModel> results)
        {
            GetSubjects();
            ResultTable = results;
            if (classes != null && term != null && session != null && subclass != null && StudentRegNo != string.Empty)
            {
                var ifisregister = StudentRegNo + HttpContext.Session.GetInt32("session") + term + HttpContext.Session.GetInt32("classes")+ HttpContext.Session.GetInt32("subclass");

                var _getUser = dbContext.Termregistrations.Include(k=>k.ApplicationUser).FirstOrDefault(h => h.IfIsRegisterB4 == ifisregister);
                if (_getUser != null)
                {
                    termreg = _getUser;
                    Fullname = _getUser.ApplicationUser.SurName + ' ' + _getUser.ApplicationUser.OtherName + ' ' + _getUser.ApplicationUser.FirstName;
                    StudentRegNo = _getUser.ApplicationUser.StudentRegNo;
                    TempData["success"] = "Student Found!!";
                    return Page();
                }
                TempData["error"] = "Student Not Found";
                return Page();
            }
            TempData["error"] = "Please select and input in the required fields!!";
            return Page();
        }
        public void GetSubjects()
        {
            Subject = dbContext.Subjects.Select(k => new SelectListItem
            {
                Text = k.Name,
                Value = k.Name
            });
        }
    }
}
