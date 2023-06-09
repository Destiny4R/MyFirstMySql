using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFirstDataAccess;
using MyFirstModel;
using MyFirstModel.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;

namespace MyFirstMySql.Pages.ComputeResult
{
    [Authorize]
    public class UploadResultModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IConfiguration configuration;
        IExcelDataReader reader;
        public ViewSelectModel ViewSelectModel { get; set; }
        public IEnumerable<SelectListItem> Subject { get; set; }
        [BindProperty]
        [Required]
        public string session { get; set; }
        [BindProperty]
        [Required]
        public string classes { get; set; }
        [BindProperty]
        [Required]
        public string subclass { get; set; }
        [BindProperty]
        [Required]
        public string term { get; set; }
        public UploadResultModel(ApplicationDbContext dbContext,
            IWebHostEnvironment hostEnvironment,
            IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.hostEnvironment = hostEnvironment;
            this.configuration = configuration;
        }

        public IActionResult OnGet()
        {
            getViewSelect();
            return Page();
        }
        public async Task<IActionResult> OnPost(IFormFile file)
        {
            try
            {
                getViewSelect();
                // Check the File is received

                if (file == null)
                    throw new Exception("File is Not Received...");


                // Create the Directory if it is not exist
                string dirPath = Path.Combine(hostEnvironment.WebRootPath, "ReceivedReports");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                // MAke sure that only Excel file is used 
                string dataFileName = Path.GetFileName(file.FileName);

                string extension = Path.GetExtension(dataFileName);

                string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

                if (!allowedExtsnions.Contains(extension))
                    throw new Exception("Sorry! This file is not allowed, make sure that file having extension as either .xls or .xlsx is uploaded.");

                // Make a Copy of the Posted File from the Received HTTP Request
                string saveToPath = Path.Combine(dirPath, dataFileName);

                using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                int ree = 0, ess=0;
                // USe this to handle Encodeing differences in .NET Core
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                // read the excel file
                using (var stream = new FileStream(saveToPath, FileMode.Open))
                {
                    if (extension == ".xls")
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    else
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                    DataSet ds = new DataSet();
                    ds = reader.AsDataSet();
                    reader.Close();
                    
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        var ClaimsId = (ClaimsIdentity)User.Identity;
                        var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
                        // Read the the Table
                        DataTable serviceDetails = ds.Tables[0];
                        for (int i = 1; i < serviceDetails.Rows.Count; i++)
                        {
                            //Check If Student is register for the Term(FORMULA)
                            var ifisregister = serviceDetails.Rows[i][1].ToString().Replace(" ", "") +session + term + classes + subclass;

                            var getuser = dbContext.Termregistrations.Include(k => k.ApplicationUser).FirstOrDefault(h => h.IfIsRegisterB4 == ifisregister);


                            //Check If Student Result in A Subject is Already register for the Term(FORMULA)
                            string IsAddedBeforeValeue = getuser.ApplicationUser.StudentRegNo + serviceDetails.Rows[i][0].ToString() + session + term + classes + subclass;

                            double a1 = (double)serviceDetails.Rows[i][2];
                            double a2 = (double)serviceDetails.Rows[i][3];
                            double test = (double)serviceDetails.Rows[i][4];
                            double exam = (double)serviceDetails.Rows[i][5];

                            var _GetResult = dbContext.ResultTables.FirstOrDefault(g => g.IsAddedBefore == IsAddedBeforeValeue);
                            if (_GetResult != null)
                            {
                                ree ++;
                            }
                            else 
                            { 
                            ResultTable result = new();
                            result.Assessment1 = a1 ;
                                result.Assessment2 = a2; 
                            result.Test = test;
                            result.Examination = exam;
                            result.Total = SD.TotalAnswer(a1,a2,test,exam);
                            result.Position = 0;
                            result.Grade = SD.CalculateGrade(SD.TotalAnswer(a1, a2, test, exam));
                            result.SubjectId = Int32.Parse(serviceDetails.Rows[i][0].ToString());
                            result.Term = term;
                            result.SessionYearId = Int32.Parse(session);
                            result.ClassesId = Int32.Parse(classes);
                            result.SubClassId = Int32.Parse(subclass);
                            result.IsAddedBefore = IsAddedBeforeValeue;
                            result.TermRegId = getuser.Id;
                                result.ExamsOfficer = claim.Value;
                                // Add the record in Database
                                await dbContext.ResultTables.AddAsync(result);
                               ess+= await dbContext.SaveChangesAsync();
                            }
                        }
                    }
                }
                TempData["success"] = $"{ree} Not Added And {ess} Successfully Added";
                return Page();
            }
            catch (Exception ex)
            {
               TempData["error"] = ex.Message;
            }
            //TempData["error"] = $"Something Happened Result Not Uploaded!";
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
