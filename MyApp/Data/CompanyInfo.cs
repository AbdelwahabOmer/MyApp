namespace MyApp.Data
{
    public class CompanyInfo
    {
        public long ID { get; set; }
        public string CMP { get; set; }
        public string Name { get; set; }
        public string Web { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? TRN { get; set; }
        public string Address { get; set; }
        public int CompanyType { get; set; }
        public string? LogoURL { get; set; }
        public string? StampURL { get; set; }
        public string? SignURL { get; set; }
    }
}
