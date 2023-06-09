using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace MyFirstModel
{
    [Index(nameof(IsAddedBefore),IsUnique = true)]
    public class ResultTable
    {
        public int Id { get; set; }
        public double Assessment1 { get; set; }
        public double Assessment2 { get; set; }
        public double Test { get; set; }
        public double Examination { get; set; }
        public double Total { get; set; }
        public int Position { get; set; }
        [MaxLength(4)]
        public string Grade { get; set; }
        [MaxLength(20)]
        public string Remark { get; set; }
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subjects Subjects { get; set; }
        public string Term { get; set; }
        public int SessionYearId { get; set; }
        [ForeignKey(nameof(SessionYearId))]
        public SessionYear SessionYear { get; set; }
        public int ClassesId { get; set; }
        [ForeignKey(nameof(ClassesId))]
        public ClassesInSchool ClassesInSchool { get; set; }
        public int SubClassId { get; set; }
        [ForeignKey(nameof(SubClassId))]
        public SubClasses SubClasses { get; set; }
        public string IsAddedBefore { get; set; }
        public int TermRegId { get; set; }
        [ForeignKey(nameof(TermRegId))]
        public TermregistrationTable TermregistrationTable { get; set; }
        public string? ExamsOfficer { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
