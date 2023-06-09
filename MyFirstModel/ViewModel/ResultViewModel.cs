using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstModel.ViewModel
{
    public class ResultViewModel
    {
        public int Id { get; set; }
        public double Assessment1 { get; set; }
        public double Assessment2 { get; set; }
        public double Test { get; set; }
        public double Examination { get; set; }
        public double Total { get; set; }
        public int Position { get; set; }
        public string Grade { get; set; }
        public string Remark { get; set; }
        public string Subjects { get; set; }
        public string Term { get; set; }
    }
}
