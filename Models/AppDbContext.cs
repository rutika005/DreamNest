using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Razorpay.Api;

namespace Aesthetica.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ContactModel> contact { get; set; }  
        public DbSet<RegisterModel> userregister { get; set; }  
        public DbSet<UserRegister> userRegister { get; set; }
        public DbSet<LoginModel> admin { get; set; }           
        public DbSet<BlogPost> blogadmin { get;  set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }  
        public DbSet<SavedPost> savedposts { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<PropertyModel> properties { get; set; }
        public DbSet<JobListing> JobListings { get; set; }
        public DbSet<Room> Rooms { get; set; }

    }
}
