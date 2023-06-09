using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFirstDataAccess;
using MyFirstModel.ViewModel;

namespace MyFirstMySql.Pages.Settings
{
    [Authorize]
    public class SetClassModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        public ViewSelectModel ViewSelectModel { get; set; }
        public SetClassModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        public void OnGet()
        {
            getViewSelect();
        }
        public void OnPost()
        {
            HttpContext.Session.SetString("term", CTSSClass.Term);
            HttpContext.Session.SetInt32("session", CTSSClass.SessionYear);
            HttpContext.Session.SetInt32("classes", CTSSClass.Classes);
            HttpContext.Session.SetInt32("subclass", CTSSClass.Subclass);
            TempData["setting"] = "Class Successfully Set!";
            getViewSelect();
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
