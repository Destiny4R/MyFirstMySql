using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFirstDataAccess;
using MyFirstModel;
using MyFirstModel.ViewModel;
using System.Security.Claims;

namespace MyFirstMySql.Pages.Admin.TermRegistration
{
    [Authorize]
    public class RegisterStudentModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        [BindProperty]
        public string StudentFullname { get; set; } = ""; 
        [BindProperty]
        public string StudentRegNo { get; set; } = "";
        public RegisterStudentModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        private void getViewSelect()
        {
            ViewSelectModel = new()
            {
                Class = dbContext.ClassesInSchools.Select(k => new SelectListItem { 
                Text = k.ClassName, Value = k.Id.ToString()
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
        public void OnGet()
        {
            getViewSelect();
        }
        public IActionResult OnPostSearch()
        {

            if (StudentRegNo != "")
            {
                var studentinfo = dbContext.ApplicationUsers.FirstOrDefault(i => i.StudentRegNo == StudentRegNo && i.UserID==SD.IsStudent);
                if (studentinfo != null)
                {
                    //Get Student Fullname
                    StudentFullname = studentinfo.SurName + ' ' + studentinfo.OtherName + ' ' + studentinfo.FirstName;
                    getViewSelect();
                    TempData["success"] = "Student found!";
                    return Page();
                }
                TempData["error"] = "Student with the bellow registration number not found!";
                getViewSelect();
                return Page();
            }
            TempData["error"] = "Please make the right selections and try again!";
            getViewSelect();
            return Page();
        }
        public IActionResult OnPostSave()
        {
            if (ModelState.IsValid)
            {
                var studentIDNO = dbContext.ApplicationUsers.FirstOrDefault(d => d.StudentRegNo == StudentRegNo && d.UserID == SD.IsStudent);
                if (studentIDNO != null)
                {
                    var ClaimsId = (ClaimsIdentity)User.Identity;
                    var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);

                    var VerifyInfo = StudentRegNo + CTSSClass.SessionYear + CTSSClass.Term + CTSSClass.Classes.ToString().Replace(" ", string.Empty) + CTSSClass.Subclass;

                    var IfstudentIsRegB4 = dbContext.Termregistrations.FirstOrDefault(d => d.IfIsRegisterB4 == VerifyInfo);
                    if (IfstudentIsRegB4 != null)
                    {
                        TempData["error"] = "Student Already Registered for the Term";
                        getViewSelect();
                        return Page();
                    }
                    TermregistrationTable term = new()
                    {
                        Term = CTSSClass.Term,
                        SessionYearId = CTSSClass.SessionYear,
                        ClassesInSchoolId = CTSSClass.Classes,
                        SubClassId = CTSSClass.Subclass,
                        ApplicationUserID = studentIDNO.Id,
                        IfIsRegisterB4 = VerifyInfo,
                        Username = claim.Value
                    };
                    dbContext.Termregistrations.Add(term);
                    dbContext.SaveChanges();
                    TempData["success"] = $"{studentIDNO.SurName + ' ' + studentIDNO.OtherName + ' ' + studentIDNO.FirstName} Successfully registered for the Term!";
                    return RedirectToPage("Index");

                }
                getViewSelect();
                TempData["error"] = "Wrong Student Registration Number, Check and Try Again";
                return RedirectToPage("Index");
            }
            getViewSelect();
            return Page();
        }
    }
}
