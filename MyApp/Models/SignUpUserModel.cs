using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class SignUpUserModel
    {
        [Required(ErrorMessage = "enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "enter your last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "enter your email")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "enter valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "enter your password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "password does not match")]
        public string Password { get; set; }

        [Required(ErrorMessage = "confirm your password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
