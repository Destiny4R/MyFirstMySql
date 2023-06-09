using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFirstDataAccess;
using MyFirstModel.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MyFirstMySql.Pages.Portal
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        [BindProperty]
        [Required]
        public string StudentRegNo { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }
        public IndexModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            getViewSelect();
        }
        public IActionResult OnPost()
        {
            //var ClaimsId = (ClaimsIdentity)User.Identity;
            //var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
            //var user = dbContext.ApplicationUsers.Find(claim.Value);
            if (ModelState.IsValid)
            {

                var isAdded = StudentRegNo + CTSSClass.SessionYear + CTSSClass.Term + CTSSClass.Classes.ToString().Replace(" ", string.Empty) + CTSSClass.Subclass;
                return RedirectToPage("ResultPage", new { isadded = isAdded });
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
