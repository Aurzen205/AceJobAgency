using System.ComponentModel.DataAnnotations;

namespace AceJobAgency.Models
{
    public class Login
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }

    }
}
