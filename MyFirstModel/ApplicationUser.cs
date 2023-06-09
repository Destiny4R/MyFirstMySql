using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace MyFirstModel
{
    [Index(nameof(StudentRegNo), IsUnique = true)]
    public class ApplicationUser: Microsoft.AspNetCore.Identity.IdentityUser
    {
        public string? StudentRegNo { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string? OtherName { get; set; }
        [Required]
        [MaxLength(50)]
        public string SurName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Gender { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Address { get; set; }
        [Required]
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Local Gov")]
        public string LocalGov { get; set; }
        [MaxLength(50)]
        [Display(Name = "Place Of Birth")]
        public string? PlaceOfBirth { get; set; }
        [MaxLength(5)]
        [Display(Name = "Blood Group")]
        public string? BloodGroup { get; set; }
        public string Passport { get; set; }
        public string UserID { get; set; }
        public string RegisterBy { get; set; }
        public long SN { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
