using Microsoft.EntityFrameworkCore;

namespace Aesthetica.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ContactModel> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Database=aesthetica;User Id=root;Password=;",
                new MySqlServerVersion(new Version(8, 0, 23)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactModel>().ToTable("Contacts"); // Maps ContactModel to Contacts table
        }
    }
}