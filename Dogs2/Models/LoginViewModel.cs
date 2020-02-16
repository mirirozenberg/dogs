using System.ComponentModel.DataAnnotations;

namespace Dogs2.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "You must enter a value for the Email field!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please insert your Password!")]
        [StringLength(10, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
