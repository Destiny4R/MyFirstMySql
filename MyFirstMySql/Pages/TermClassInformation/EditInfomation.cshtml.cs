using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstModel.ViewModel;
using MyFirstModel;
using MyFirstDataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MyFirstMySql.Pages.TermClassInformation
{
    [Authorize]
    public class EditInfomationModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public ClassGeneralTable ClassGeneral { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public EditInfomationModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult OnGet(int id)
        {
            if(id < 1)
            {
                return RedirectToPage("Index");
            }
            getViewSelect();
            ClassGeneral = dbContext.ClassGeneralTables.FirstOrDefault(h => h.Id == id);
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ClassGeneral != null)
            {
                var ClaimsId = (ClaimsIdentity)User.Identity;
                var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);

                string ifIsregister = ClassGeneral.SessionYearId + ClassGeneral.Term + ClassGeneral.ClassesId + ClassGeneral.SubClassId;
                var GetInfo = dbContext.ClassGeneralTables.FirstOrDefault(k => k.Id == ClassGeneral.Id);

                GetInfo.Term = ClassGeneral.Term;
                GetInfo.SessionYearId = ClassGeneral.SessionYearId;
                GetInfo.ClassesId = ClassGeneral.ClassesId;
                GetInfo.SubClassId = ClassGeneral.SubClassId;
                GetInfo.Next_Term_Fees = ClassGeneral.Next_Term_Fees;
                GetInfo.TermEnd = ClassGeneral.TermEnd;
                GetInfo.NextTermStart = ClassGeneral.NextTermStart;
                GetInfo.TotalAttendance = ClassGeneral.TotalAttendance;
                GetInfo.ClassTeacher = ClassGeneral.ClassTeacher;
                GetInfo.IfIsRegisterB4 = ifIsregister;
                GetInfo.ExamOfficerID = claim.Value;

                dbContext.Update(GetInfo);
                dbContext.SaveChanges();
                TempData["success"] = "Information Successfully Updated!";
                return RedirectToPage("Index");
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
