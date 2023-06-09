using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstModel
{
    public class RemarkPosition
    {
        public int Id { get; set; }
        public double? Student_Attendance { get; set; } = 0;
        [MaxLength(10)]
        public string? Absent { get; set; } = null;
        public double? Total_Marks_Obtain { get; set; } = 0;
        [MaxLength(10)]
        public string? Position_In_Class { get; set; } = null;
        [MaxLength(250)]
        public string? General_Remark { get; set; } = null;
        [MaxLength(250)]
        public string? Principal_Remark { get; set; } = null; 
        [MaxLength(250)]
        public string? IfIsRegisterB4 { get; set; } = null;
        [MaxLength(250)]
        public string? AddedBy { get; set; } = null;
        public int TermRegId { get; set; }
        [ForeignKey(nameof(TermRegId))]
        public TermregistrationTable TermregistrationTable { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
