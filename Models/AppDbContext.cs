using Microsoft.EntityFrameworkCore;

namespace Aesthetica.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ContactModel> contact { get; set; }  // Maps to Contacts table
        public DbSet<RegisterModel> userregister { get; set; }  // Maps to Users table
    }
}
