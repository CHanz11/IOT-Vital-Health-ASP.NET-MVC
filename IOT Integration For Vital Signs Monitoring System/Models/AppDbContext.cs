using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IOT_Integration_For_Vital_Signs_Monitoring_System.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Patients> Patient { get; set; }
        public DbSet<Queues> Queue { get; set; }
        public DbSet<Records> Record { get; set; }
    }
}
