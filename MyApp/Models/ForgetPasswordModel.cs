using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class ForgetPasswordModel
    {
        [Required, EmailAddress, Display(Name = "Registered Email")]
        public string Email { get; set; }
        public bool EmailSent { get; set; }
    }
}
