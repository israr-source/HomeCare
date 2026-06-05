using HomeCare.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeCare.Models
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // CATEGORY TABLE
        public DbSet<ServiceCategory> ServiceCategories { get; set; }

        // SERVICE TABLE
        public DbSet<Service> Services { get; set; }

        // BOOKING TABLE
        public DbSet<Booking> Bookings { get; set; }

        // REVIEW TABLE
        public DbSet<Review> Reviews { get; set; }
    }
}