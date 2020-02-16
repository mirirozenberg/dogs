using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dogs2.Models
{
    public class RegistrationViewModel
    {
        [Key]
        public int userId { get; set; }

        [Required(ErrorMessage = "You must enter a value for the Name field!")]
        [StringLength(15, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must enter a value for the Mobile field!")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Mobile no not valid")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "You must enter a value for the Email field!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must enter a value for the Password field!")]
        [StringLength(10, MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "You must enter a value for the Confirm Password field!")]
        [NotMapped]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
