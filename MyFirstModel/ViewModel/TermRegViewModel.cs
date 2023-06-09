using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MyFirstModel.ViewModel
{
    public class TermRegViewModel
    {
        [ValidateNever]
        public int Id { get; set; }
        [ValidateNever]
        public string ApplicationUserId { get; set; }
        [Required]
        [Display(Name = "Session/Year")]
        public int SessionYearId { get; set; }
        [Required]
        [Display(Name = "Class")]
        public int ClassesInSchoolId { get; set; }
        [Required]
        [Display(Name = "Sub-Class")]
        public int SubClassId { get; set; }
        [Required]
        public string Term { get; set; }
    }
}
