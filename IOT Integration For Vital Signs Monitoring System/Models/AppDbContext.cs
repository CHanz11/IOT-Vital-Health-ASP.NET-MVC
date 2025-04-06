using Microsoft.EntityFrameworkCore;

namespace IOT_Integration_For_Vital_Signs_Monitoring_System.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Patients> Patient { get; set; }
        public DbSet<Queues> Queue { get; set; }
        public DbSet<Records> Record { get; set; }
    }
}
