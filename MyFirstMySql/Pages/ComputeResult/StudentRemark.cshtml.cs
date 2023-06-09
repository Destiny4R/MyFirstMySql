using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyFirstDataAccess;
using MyFirstModel;
using System.Security.Claims;

namespace MyFirstMySql.Pages.ComputeResult
{
    [Authorize]
    public class StudentRemarkModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public RemarkPosition remark { get; set; }
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
        public StudentRemarkModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult OnGet(int id)
        {
            remark = dbContext.RemarkPositions.Include(n => n.TermregistrationTable).ThenInclude(k => k.ApplicationUser).Include(k => k.TermregistrationTable).ThenInclude(m => m.ClassesInSchool).Include(k => k.TermregistrationTable).ThenInclude(m => m.SessionYear).Include(k => k.TermregistrationTable).ThenInclude(m => m.SubClasses).Where(j => j.TermRegId == id).FirstOrDefault();
            if(remark == null)
            {
                return RedirectToPage("Index");
            }
            StudentRegNo = remark.TermregistrationTable.ApplicationUser.StudentRegNo;
            Fullname = remark.TermregistrationTable.ApplicationUser.SurName + " " + remark.TermregistrationTable.ApplicationUser.OtherName + " " + remark.TermregistrationTable.ApplicationUser.FirstName;
            session = remark.TermregistrationTable.SessionYear.Name;
            classes = remark.TermregistrationTable.ClassesInSchool.ClassName;
            subclass = remark.TermregistrationTable.SubClasses.ClassName;
            term = remark.TermregistrationTable.Term;
            if (remark.Position_In_Class == null || remark.Position_In_Class=="0")
            {
                TempData["error"] = "Please Add Result and Compute Position in Class";
                return RedirectToPage("/ComputeResult/Index");
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            var ClaimsId = (ClaimsIdentity)User.Identity;
            var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);

            if (remark.Id >0)
            {
                var model = dbContext.RemarkPositions.FirstOrDefault(k => k.Id == remark.Id);
                
                model.Student_Attendance = remark.Student_Attendance;
                model.Absent = remark.Absent;
                model.Total_Marks_Obtain = remark.Total_Marks_Obtain;
                model.Position_In_Class = remark.Position_In_Class;
                model.General_Remark = remark.General_Remark;
                model.Principal_Remark = remark.Principal_Remark;
                model.AddedBy = claim.Value;

                dbContext.RemarkPositions.Update(model);
                dbContext.SaveChanges();
                TempData["success"] = "Information Successfully Updated!";
                return RedirectToPage("Index");
            }

            TempData["error"] = "Something happened, Updating failed!";
            return Page();
        }
    }
}
