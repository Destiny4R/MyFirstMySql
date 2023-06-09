using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyFirstDataAccess;
using MyFirstModel;

namespace MyFirstMySql.Pages.Portal
{
    public class ResultPageModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public RemarkPosition remark {  get; set; }
        public IEnumerable<ResultTable> result { get; set; }
        public ClassGeneralTable ClassGeneral { get; set; }
        public ResultPageModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public double TotalMarkObtainable = 0, TotalAVERAGESCORE = 0, TotalAVERAGE2 = 0, remarkable=0, promotionalAverage = 0; 
        public IActionResult OnGet(string isadded)
        {
            remark = dbContext.RemarkPositions.Include(n => n.TermregistrationTable).ThenInclude(k => k.ApplicationUser).Include(n => n.TermregistrationTable).ThenInclude(k => k.ClassesInSchool).Include(n => n.TermregistrationTable).ThenInclude(k => k.SessionYear).Include(n => n.TermregistrationTable).ThenInclude(k => k.SubClasses).Where(u => u.IfIsRegisterB4 == isadded).FirstOrDefault();
            if (remark == null)
            {
                TempData["message"] = "No Result Found For The Provided Student Information. Check and Try Again!!!!";
                return RedirectToPage("Index");
            }
            result = dbContext.ResultTables.Include(k=>k.Subjects).Where(k=>k.TermRegId==remark.TermRegId).ToList();
            //Total Subjects Scores
            foreach (var item in result)
            {
                
            }

            TotalMarkObtainable = result.Count() * 100;
            var getstudentInClass = dbContext.Termregistrations.Include(o => o.ApplicationUser).Where(k => k.SubClassId == remark.TermregistrationTable.SubClassId && k.Term == remark.TermregistrationTable.Term && k.SessionYearId == remark.TermregistrationTable.SessionYearId && k.ClassesInSchoolId == remark.TermregistrationTable.ClassesInSchoolId).ToList();
            double NumberOfStudents = getstudentInClass.Count();
            //GET AVERAGE SCORES
            var gettotal = dbContext.ResultTables.Where(k=>k.SubClassId==remark.TermregistrationTable.SubClassId && k.Term== remark.TermregistrationTable.Term && k.SessionYearId== remark.TermregistrationTable.SessionYearId && k.ClassesId== remark.TermregistrationTable.ClassesInSchoolId).ToList();
            double addtotal=0;
            foreach (var item in gettotal)
            {
                addtotal+= item.Total;
            }
            TotalAVERAGESCORE = addtotal / NumberOfStudents;

            //GET AVERAGE AGE
            int age = 0;
            foreach (var item in getstudentInClass)
            {
                age += DateTime.Now.Year - item.ApplicationUser.DateOfBirth.Year;
            }
            TotalAVERAGE2 = age / NumberOfStudents;
            
            string ifIsregister = remark.TermregistrationTable.SessionYearId + remark.TermregistrationTable.Term + remark.TermregistrationTable.ClassesInSchool.Id + remark.TermregistrationTable.SubClassId;

            ClassGeneral = dbContext.ClassGeneralTables.FirstOrDefault(u=>u.IfIsRegisterB4== ifIsregister);

            //IF STUDENT QUALIFIED TO BE PROMOTED TO THE NEXT CLASS
            var Promotional = dbContext.ResultTables.Include(k=>k.TermregistrationTable).ThenInclude(K=>K.ApplicationUser).Where(h=>h.SubClassId== remark.TermregistrationTable.SubClassId && h.SessionYearId== remark.TermregistrationTable.SessionYearId && h.TermregistrationTable.ApplicationUser.StudentRegNo== remark.TermregistrationTable.ApplicationUser.StudentRegNo).ToList();
            
            if (Promotional.Count() > 0)
            {
                double val = 0, _partInTerm;
                double totalfromterms = dbContext.Termregistrations.Include(k => k.ApplicationUser).Where(h => h.SubClassId == remark.TermregistrationTable.SubClassId && h.SessionYearId == remark.TermregistrationTable.SessionYearId && h.ApplicationUser.StudentRegNo == remark.TermregistrationTable.ApplicationUser.StudentRegNo).ToList().Count();
                foreach (var item in Promotional)
                {
                    val += item.Total;
                }
                _partInTerm = 100 * Promotional.Count();
                promotionalAverage = (val / _partInTerm) * 100;
                promotionalAverage = Math.Round(promotionalAverage, 2);


                //Get Remark comment for principal and class teacher
                remarkable = (val / TotalMarkObtainable) * 100;
            }
            
            return Page();
        }
    }
}
