using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class SignUpUserModel
    {
        [Required, Display(Name = "Company Name"), MinLength(3, ErrorMessage = "minimum 3 chars")]
        public string Name { get; set; }

        [Required, Display(Name = "Website")]
        public string Web { get; set; }

        [Required, EmailAddress, Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required, Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required, Display(Name = "Address")]
        public string Address { get; set; }

        [Required, Display(Name = "Company Activity")]
        public int CompanyType { get; set; }


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
