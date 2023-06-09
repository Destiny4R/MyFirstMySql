using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MyFirstDataAccess;
using MyFirstModel;
using MyFirstModel.ViewModel;
using System.Security.Claims;

namespace MyFirstMySql.Controllers
{
    public class apiController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IWebHostEnvironment hostEnvironment;

        public apiController(ApplicationDbContext dbContext,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment hostEnvironment)
        {
            this.dbContext = dbContext;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.hostEnvironment = hostEnvironment;
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login");
        }

        [HttpPost]
        public JsonResult StudentData()
        {
            try
            {
                //var user = await userManager.GetUserAsync(User);
                IEnumerable<ApplicationUser> userData = dbContext.ApplicationUsers.Where(k => k.UserID == SD.IsStudent).ToList();
                userData = userData.OrderByDescending(k => k.CreateDate).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = userData.Where(m => m.FirstName.Contains(searchValue)
                                                || m.Gender.Contains(searchValue)
                                                || m.SurName.Contains(searchValue)
                                                || m.StudentRegNo.Contains(searchValue)
                                                || m.OtherName.Contains(searchValue));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public JsonResult StaffData()
        {
            try
            {
                //var user = await userManager.GetUserAsync(User);
                IEnumerable<ApplicationUser> userData = dbContext.ApplicationUsers.Where(k=>k.UserID==SD.IsEmplyee).ToList();
                userData = userData.OrderByDescending(k => k.CreateDate).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = userData.Where(m => m.FirstName.Contains(searchValue)
                                                || m.Gender.Contains(searchValue)
                                                || m.SurName.Contains(searchValue)
                                                || m.StudentRegNo.Contains(searchValue)
                                                || m.OtherName.Contains(searchValue));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public JsonResult StudentTermData()
        {
            try
            {
                //var user = await userManager.GetUserAsync(User);
                IEnumerable<TermregistrationTable> userData = dbContext.Termregistrations.Include(k=>k.ApplicationUser).Include(k => k.SessionYear).Include(k => k.ClassesInSchool).Include(k => k.SubClasses).ToList();
                userData = userData.OrderByDescending(k => k.CreatedDate).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = userData.Where(m => m.Term.Contains(searchValue)
                                                || m.ApplicationUser.UserName.Contains(searchValue)
                                                || m.ApplicationUser.SurName.Contains(searchValue)
                                                || m.ApplicationUser.FirstName.Contains(searchValue)
                                                || m.ApplicationUser.OtherName.Contains(searchValue)
                                                || m.ClassesInSchool.ClassName.Contains(searchValue)
                                                || m.SubClasses.ClassName.Contains(searchValue)
                                                || m.SessionYear.Name.Contains(searchValue));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public JsonResult SendResult(List<ResultViewModel> resultsExams)
        {
            if (HttpContext.Session.GetInt32("classes") > 0)
            {
                int upload = 0, notupload = 0;
                var ClaimsId = (ClaimsIdentity)User.Identity;
                var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
                foreach (var result in resultsExams)
                {

                    var getStudentInfo = dbContext.Termregistrations.Include(m=>m.ApplicationUser).FirstOrDefault(k => k.Id == result.Id);
                    if (getStudentInfo != null)
                    {
                        //get student reg number ;
                        //
                        string VerifyInfoStudent = getStudentInfo.ApplicationUser.StudentRegNo + HttpContext.Session.GetInt32("session") + HttpContext.Session.GetString("term") + HttpContext.Session.GetInt32("classes") + HttpContext.Session.GetInt32("subclass");

                        //If subject is added for the very student variable
                        var getsubject = dbContext.Subjects.FirstOrDefault(k=>k.Name==result.Subjects);

                        string IsAddedBeforeValeue = getStudentInfo.ApplicationUser.StudentRegNo + getsubject.Id + HttpContext.Session.GetInt32("session") + HttpContext.Session.GetString("term") + HttpContext.Session.GetInt32("classes") + HttpContext.Session.GetInt32("subclass");

                        var _GetResult = dbContext.ResultTables.FirstOrDefault(g => g.IsAddedBefore == IsAddedBeforeValeue);
                        if (_GetResult == null)
                        {
                            ResultTable results = new()
                            {
                                Assessment1 = result.Assessment1,
                                Assessment2 = result.Assessment2,
                                Test = result.Test,
                                Examination = result.Examination,
                                Total = result.Total,
                                Position = 0,
                                Grade = result.Grade,
                                Remark = result.Remark,
                                SubjectId = getsubject.Id,
                                Term = getStudentInfo.Term,
                                SessionYearId = getStudentInfo.SessionYearId,
                                ClassesId = getStudentInfo.ClassesInSchoolId,
                                SubClassId = getStudentInfo.SubClassId,
                                TermRegId = getStudentInfo.Id,
                                IsAddedBefore = IsAddedBeforeValeue,
                                ExamsOfficer = claim.Value
                            };
                            dbContext.ResultTables.Add(results);
                            dbContext.SaveChanges();
                            upload++;
                        }
                        else
                        {
                            notupload++;
                        }
                    }
                }
                string message = string.Empty, message2 = string.Empty;
                if (notupload > 0)
                {
                    message = " And " + notupload + " Result(s) not saved!";
                }
                if (upload > 0)
                {
                    message2 = $"{upload} Result successfully added";
                }
                //TempData["success"] = "Result successfully added!";
                return Json($"{message2} {message}");
            }
            return Json($"{"Please Set Class Before Posting Result"}");
        }

        [HttpPost]
        public JsonResult PerClassRegTermStudent()
        {
            try
            {
                //var user = await userManager.GetUserAsync(User);
                IEnumerable<TermregistrationTable> userData = dbContext.Termregistrations.Include(k => k.ApplicationUser).Include(k => k.SessionYear).Include(k => k.ClassesInSchool).Include(k => k.SubClasses).ToList();
                userData = userData.OrderByDescending(k => k.CreatedDate).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = userData.Where(m => m.Term.Contains(searchValue)
                                                || m.ApplicationUser.UserName.Contains(searchValue)
                                                || m.ApplicationUser.SurName.Contains(searchValue)
                                                || m.ApplicationUser.FirstName.Contains(searchValue)
                                                || m.ApplicationUser.OtherName.Contains(searchValue)
                                                || m.ClassesInSchool.ClassName.Contains(searchValue)
                                                || m.SubClasses.ClassName.Contains(searchValue)
                                                || m.SessionYear.Name.Contains(searchValue));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //API FOR UPDATING POSITION IN SUBJECT
        public JsonResult UpdatePosition(List<ResultTable> Results)
        {
            string message = "";
            if (Results == null || Results.Count == 0)
            {
                message = "Data Not";
            }
            if (Results != null && Results.Count > 0)
            {
                message = "Data";
            }
            //Check for NULL.
            if (Results != null)
            {
                //Loop and insert records.
                foreach (var cuzt in Results)
                {
                    var getinfo = dbContext.ResultTables.FirstOrDefault(h => h.Id == cuzt.Id);
                    if (getinfo != null)
                    {
                        getinfo.Position = cuzt.Position;
                        dbContext.Update(getinfo);
                        dbContext.SaveChanges();
                    }
                }
                return Json(message);
            }
            return Json(message);
        }
        //API FOR UPDATING POSITION IN CLASS
        public JsonResult UpdatePositionInClass(List<RemarkPosition> Remarks)
        {
            string message = "";
            if (Remarks == null || Remarks.Count == 0)
            {
                message = "Data Not";
            }
            if (Remarks != null && Remarks.Count > 0)
            {
                message = "Data";
            }
            //Check for NULL.
            if (Remarks != null)
            {
                //Loop and insert records.
                foreach (var cuzt in Remarks)
                {
                    var getstudent = dbContext.Termregistrations.Include(k => k.ApplicationUser).Where(k => k.Id == cuzt.TermRegId).FirstOrDefault();

                    var ifisregister = getstudent.ApplicationUser.StudentRegNo + HttpContext.Session.GetInt32("session") + HttpContext.Session.GetString("term") + HttpContext.Session.GetInt32("classes") + HttpContext.Session.GetInt32("subclass");

                    var getinfo = dbContext.RemarkPositions.FirstOrDefault(h => h.IfIsRegisterB4 == ifisregister);
                    if (getinfo != null)
                    {
                        getinfo.Position_In_Class = cuzt.Position_In_Class;
                        dbContext.RemarkPositions.Update(getinfo);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        RemarkPosition remark = new()
                        {
                            Position_In_Class = cuzt.Position_In_Class,
                            IfIsRegisterB4 = ifisregister,
                            TermRegId = getstudent.Id
                        };
                        dbContext.RemarkPositions.Add(remark);
                        dbContext.SaveChanges();
                    }
                }
                return Json(message);
            }
            return Json(message);
        }
        [HttpPost]
        public JsonResult GeneralInformation()
        {
            try
            {
                //var user = await userManager.GetUserAsync(User);
                IEnumerable<ClassGeneralTable> userData = dbContext.ClassGeneralTables.Include(k => k.SessionYear).Include(k => k.ClassesInSchool).Include(k => k.SubClasses).ToList();
                userData = userData.OrderByDescending(k => k.CreateDate).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = userData.Where(m => m.Term.Contains(searchValue)
                                                || m.Term.Contains(searchValue)
                                                || m.ClassesInSchool.ClassName.Contains(searchValue)
                                                || m.SubClasses.ClassName.Contains(searchValue)
                                                || m.SessionYear.Name.Contains(searchValue));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpDelete]
        public async Task<JsonResult> UserDelete(string id)
        {
            var obj = await userManager.FindByIdAsync(id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while delete" });
            }

            string deleteOldImage = hostEnvironment.WebRootPath+ obj.Passport;
            if (System.IO.File.Exists(deleteOldImage))
            {
                System.IO.File.Delete(deleteOldImage);
            }
            string status = "Student";
            
                var dBRole = userManager.GetRolesAsync(obj).Result.FirstOrDefault();
                if (dBRole != null)
                {
                    await userManager.RemoveFromRoleAsync(obj, dBRole);
                status = "Employee";
                }
            var result = await userManager.DeleteAsync(obj);
            if (result.Succeeded)
            {
                return Json(new { success = true, message = $"{status} Successfully Deleted" });
            }

            return Json(new { success = false, message = "Something went wrong" });
        }

        [HttpDelete]
        public JsonResult DeleteTermReg(int id)
        {
            var student = dbContext.Termregistrations.FirstOrDefault(k=>k.Id==id);
            if (student == null)
            {
                return Json(new { success = false, message = "Error while delete" });
            }
            dbContext.Remove(student);
            dbContext.SaveChanges();

            return Json(new { success = true, message = "Student Successfully Removed From the Term" });
        }
    }
}
