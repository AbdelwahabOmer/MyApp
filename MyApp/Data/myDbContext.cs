using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace MyApp.Data
{
    public class myDbContext : IdentityDbContext<ApplicationUser>
    {
        public myDbContext(DbContextOptions<myDbContext> options) : base(options)
        {

        }

        public DbSet<Books> Books { get; set; }
    }
}
