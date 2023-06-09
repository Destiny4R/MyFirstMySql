using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFirstDataAccess;
using MyFirstModel;
using MyFirstModel.ViewModel;
using System.Security.Claims;

namespace MyFirstMySql.Pages.Admin.TermRegistration
{
    [Authorize]
    public class EditRegistrationModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public TermRegViewModel Termreg { get; set; }
        //[BindProperty]
        //public CTSSClass CTSSClass { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        [BindProperty]
        public string StudentFullname { get; set; } = "";
        [BindProperty]
        public string StudentRegNo { get; set; } = "";
        public EditRegistrationModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult OnGet( int id)
        {
            Termreg = dbContext.Termregistrations.Where(k => k.Id == id).Include(k=>k.ApplicationUser).Select(k => new TermRegViewModel
            {
                Id = k.Id,
                SessionYearId = k.SessionYearId,
                ApplicationUserId = k.ApplicationUserID,
                ClassesInSchoolId = k.ClassesInSchoolId,
                SubClassId = k.SubClassId,
                Term = k.Term
            }).FirstOrDefault();
            if (Termreg == null)
            {
               return RedirectToPage("Index");
            }
            var getinfo = dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == Termreg.ApplicationUserId);
            StudentFullname = getinfo.SurName + " " + getinfo.OtherName + " " + getinfo.FirstName;
            StudentRegNo = getinfo.StudentRegNo;
            getViewSelect();
            return Page();
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var studentIDNO = dbContext.ApplicationUsers.FirstOrDefault(d => d.StudentRegNo == StudentRegNo && d.UserID == SD.IsStudent);
                if (studentIDNO != null)
                {
                    var ClaimsId = (ClaimsIdentity)User.Identity;
                    var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);

                    var VerifyInfo = StudentRegNo + Termreg.SessionYearId + Termreg.Term + Termreg.ClassesInSchoolId.ToString().Replace(" ", string.Empty) + Termreg.SubClassId;

                    var IfstudentIsRegB4 = dbContext.Termregistrations.FirstOrDefault(d => d.Id == Termreg.Id);
                    if (IfstudentIsRegB4 == null)
                    {
                        TempData["error"] = "Student Term Registration Failed!";
                        return RedirectToPage("Index");
                    }
                    IfstudentIsRegB4.Term = Termreg.Term;
                    IfstudentIsRegB4.SessionYearId = Termreg.SessionYearId;
                    IfstudentIsRegB4.ClassesInSchoolId = Termreg.ClassesInSchoolId;
                    IfstudentIsRegB4.SubClassId = Termreg.SubClassId;
                    IfstudentIsRegB4.IfIsRegisterB4 = VerifyInfo;
                    IfstudentIsRegB4.Username = claim.Value;

                    dbContext.Termregistrations.Update(IfstudentIsRegB4);
                    dbContext.SaveChanges();
                    TempData["success"] = $"{studentIDNO.SurName + ' ' + studentIDNO.OtherName + ' ' + studentIDNO.FirstName} Successfully Updated!";
                    return RedirectToPage("Index");
                }
            }
            getViewSelect();
            return Page();
        }
        private void getViewSelect()
        {
            ViewSelectModel = new()
            {
                Class = dbContext.ClassesInSchools.Select(k => new SelectListItem
                {
                    Text = k.ClassName,
                    Value = k.Id.ToString()
                }),
                SubClass = dbContext.SubClasses.Select(k => new SelectListItem
                {
                    Text = k.ClassName,
                    Value = k.Id.ToString()
                }),
                SessionYearz = dbContext.SessionYears.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
    }
}
