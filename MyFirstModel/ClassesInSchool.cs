using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstModel
{
    [Index(nameof(ClassName), IsUnique = true)]
    public class ClassesInSchool
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        //public string? ApplicationUserId { get; set; } = null;
        //[ForeignKey("ApplicationUserId")]
        //public ApplicationUser ApplicationUser { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
