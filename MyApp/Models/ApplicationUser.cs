using Microsoft.AspNetCore.Identity;

namespace MyApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string CMP { get; set; }
    }
}
