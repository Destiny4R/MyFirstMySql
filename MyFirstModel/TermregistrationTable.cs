using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyFirstModel
{
    public class TermregistrationTable
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string ApplicationUserID { get; set; }
        [ForeignKey(nameof(ApplicationUserID))] 
        public ApplicationUser ApplicationUser { get; set; }
        public int SessionYearId { get; set; }
        [ForeignKey(nameof(SessionYearId))]
        public SessionYear SessionYear { get; set; }
        public int ClassesInSchoolId { get; set; }
        [ForeignKey(nameof(ClassesInSchoolId))]
        public ClassesInSchool ClassesInSchool { get; set; }
        public int SubClassId { get; set; }
        [ForeignKey(nameof(SubClassId))]
        public SubClasses SubClasses { get; set; }
        [MaxLength(50)]
        public string Term { get; set; }
        [MaxLength(250)]
        public string? IfIsRegisterB4 { get; set; }
        [MaxLength(50)]
        public string Username { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [NotMapped]
        public ResultTable ResultTable { get; set; }
    }
}
