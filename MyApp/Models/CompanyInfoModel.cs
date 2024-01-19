using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class CompanyInfoModel
    {
        public long ID { get; set; }

        public string CMP { get; set; }

        [Required, Display(Name = "Company Name"), MinLength(3, ErrorMessage = "minimum 3 chars")]
        public string Name { get; set; }

        [Required,Display(Name = "Website")]
        public string Web { get; set; }

        [Required,EmailAddress, Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required, Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required, Display(Name = "TAX Number")]
        public string TRN { get; set; }

        [Required, Display(Name = "Address")]
        public string Address { get; set; }

        [Required, Display(Name = "Company Activity")]
        public int CompanyType { get; set; }

        [Required,Display(Name = "Logo photo")]
        public IFormFile LogoPhoto { get; set; }
        public string LogoURL { get; set; }

        [Required,Display(Name = "Stamp photo")]
        public IFormFile StampPhoto { get; set; }
        public string StampURL { get; set; }

        [Required,Display(Name = "Signature photo")]
        public IFormFile SignPhoto { get; set; }
        public string SignURL { get; set; }
    }
}
