using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFirstDataAccess;
using MyFirstModel;
using MyFirstModel.ViewModel;
using System.Security.Claims;

namespace MyFirstMySql.Pages.TermClassInformation
{
    [Authorize]
    public class AddInformationModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public ClassGeneralTable ClassGeneral { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public AddInformationModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            getViewSelect();
        }
        public IActionResult OnPost()
        {
            if (ClassGeneral != null)
            {
                var ClaimsId = (ClaimsIdentity)User.Identity;
                var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);

                string ifIsregister = ClassGeneral.SessionYearId + ClassGeneral.Term + ClassGeneral.ClassesId+ ClassGeneral.SubClassId;
                var check = dbContext.ClassGeneralTables.FirstOrDefault(k => k.IfIsRegisterB4 == ifIsregister);
                if (check != null)
                {
                    TempData["error"] = "Information Already Added For the Selected Term";
                    return RedirectToPage("Index");
                }
                ClassGeneralTable BasicInfo = new()
                {
                    Term = ClassGeneral.Term,
                    SessionYearId = ClassGeneral.SessionYearId,
                    ClassesId = ClassGeneral.ClassesId,
                    SubClassId = ClassGeneral.SubClassId,
                    Next_Term_Fees = ClassGeneral.Next_Term_Fees,
                    TermEnd = ClassGeneral.TermEnd,
                    NextTermStart = ClassGeneral.NextTermStart,
                    TotalAttendance = ClassGeneral.TotalAttendance,
                    ClassTeacher = ClassGeneral.ClassTeacher,
                    IfIsRegisterB4 = ifIsregister,
                    ExamOfficerID = claim.Value,

                };
                dbContext.ClassGeneralTables.Add(BasicInfo);
                dbContext.SaveChanges();
                TempData["success"] = "Information Successfully submitted";
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
