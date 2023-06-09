using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFirstDataAccess;
using MyFirstModel;
using MyFirstModel.ViewModel;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace MyFirstMySql.Pages.ComputeResult
{
    [Authorize]
    public class EditResultModel : PageModel
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
        [BindProperty]
        public ResultTable ResultExam { get; set; }
        public EditResultModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            GetSubjects();
            var info = dbContext.ResultTables.Include(j=>j.TermregistrationTable).ThenInclude(k=>k.ApplicationUser).FirstOrDefault(k=>k.Id == id);
            if (info != null)
            {
                ResultExam = info;
                StudentRegNo = info.TermregistrationTable.ApplicationUser.StudentRegNo;
                Fullname = info.TermregistrationTable.ApplicationUser.SurName + " " + info.TermregistrationTable.ApplicationUser.OtherName + " " + info.TermregistrationTable.ApplicationUser.FirstName;
            }
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
        }
        public IActionResult OnPost()
        {
            GetSubjects();
            if (ResultExam.Id>0 && ResultExam.Remark!=null)
            {
                var getdata = dbContext.ResultTables.FirstOrDefault(k => k.Id == ResultExam.Id);
                if (getdata != null)
                {
                    getdata.Assessment1 = ResultExam.Assessment1;
                    getdata.Assessment2 = ResultExam.Assessment2;
                    getdata.Test = ResultExam.Test;
                    getdata.Examination = ResultExam.Examination;
                    getdata.Total = ResultExam.Total;
                    getdata.Grade = ResultExam.Grade;
                    getdata.Remark = ResultExam.Remark;
                    getdata.Position = ResultExam.Position;

                    dbContext.Update(getdata);
                    int result = dbContext.SaveChanges();
                    if(result > 0)
                    {
                        TempData["success"] = "Result Successfully Updated!";
                        return RedirectToPage("Index");
                    }
                }
            }
            TempData["error"] = "Result Not Updated!";
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
