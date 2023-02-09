using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Runtime.ExceptionServices;

namespace AceJobAgency.Models
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }
    public class Membership : IdentityUser
    {


        [Required, MinLength(3, ErrorMessage = "First Name must be at least 3 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required, MinLength(3, ErrorMessage = "Last Name must be at least 3 characters")]
        [Display(Name = "Last Name")]

        public string LastName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Gender")]
        [EnumDataType(typeof(Gender))]

        public Gender Gender { get; set; }

        [Required, MinLength(9, ErrorMessage = "NRIC must consist of at least 9 characters")]
        [Display(Name = "NRIC")]

        public string NRIC { get; set; } = string.Empty;


        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; } = string.Empty;

        [Required,MinLength(12,ErrorMessage = "Password must have a minimum of 12 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}$",
        ErrorMessage = "Passwords must be at least 12 characters long and contain at least an uppercase letter, lower case letter, digit and a symbol")]
        [Display(Name = "Password")]
        
        [DataType(DataType.Password)]


        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password),ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [Column(TypeName = "date")]

        public DateTime DateOfBirth { get; set; } = new DateTime(DateTime.Now.Year);


        [MaxLength(50)]
        [Display(Name = "Resume")]

        public string? ResumeURL { get; set; }


        [Required]
        [Display(Name = "WhoamI")]

        public string WhoamI{ get; set; } = string.Empty;











    }
}
