using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Razorpay.Api;

namespace Aesthetica.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ContactModel> contact { get; set; }  // Maps to Contacts table
        public DbSet<RegisterModel> userregister { get; set; }  // Maps to Users table
        public DbSet<UserRegister> userRegister { get; set; }
        public DbSet<LoginModel> admin { get; set; }           
        public DbSet<BlogPost> blogadmin { get;  set; }
        //public DbSet<PropertyModel> Properties { get; set; }  // For Property Table
        //public DbSet<PaymentViewModel> Payments { get; set; }  // For Payments Table
    }
}
