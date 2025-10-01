// MuniLK.Infrastructure.Data/ApplicationIdentityDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // Needed for IdentityUser/IdentityRole

namespace MuniLK.Infrastructure.Data
{
    // If you extend IdentityUser with your own properties, use your custom user type:
    // public class ApplicationIdentityDbContext : IdentityDbContext<YourCustomUserType, YourCustomRoleType, string>
    // Otherwise, for default IdentityUser/IdentityRole:
    public class ApplicationIdentityDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
            : base(options)
        {
        }

        // You can override OnModelCreating here if you need to add custom configurations
        // for your Identity tables (e.g., adding indexes to custom user properties)
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}