using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstModel.ViewModel
{
	public class ApplicationUserViewModel
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		[Required]
		[MaxLength(50)]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }
		[MaxLength(50)]
		[Display(Name = "Other Name")]
		public string? OtherName { get; set; }
		[Required]
		[MaxLength(50)]
		public string SurName { get; set; }
		[Required]
		[MaxLength(11)]
		[MinLength(11)]
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }
		[Required]
		public string Gender { get; set; }
		[Required]
		[MaxLength(150)]
		public string Address { get; set; }
		[Required]
		[Display(Name = "Date Of Birth")]
		public DateTime DateOfBirth { get; set; }
		[Required]
		public string State { get; set; }
		[Required]
		public string LocalGov { get; set; }
		[MaxLength(50)]
		[Display(Name = "Place Of Birth")]
		public string? PlaceOfBirth { get; set; }
		[MaxLength(5)]
		[Display(Name = "Blood Group")]
		public string? BloodGroup { get; set; }
		public string Passport { get; set; }
		[ValidateNever]
		public string Password { get; set; }
		[Required]
		public string UserRole { get; set; }
        public long SN { get; set; }
    }
}
