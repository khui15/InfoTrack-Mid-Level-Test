using Microsoft.EntityFrameworkCore;

namespace InfoTrack_Mid_Level.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Bookings> Bookings { get; set; }
    }
}
