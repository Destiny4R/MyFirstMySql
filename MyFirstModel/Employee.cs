using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyFirstModel
{
    [Index(nameof(Email), IsUnique = true)]
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:50, MinimumLength =2)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 11, MinimumLength = 11)]
        public string Phone { get; set; }
        [StringLength(maximumLength: 250)]
        public string? Address { get; set; }
    }
}
