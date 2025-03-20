using Microsoft.EntityFrameworkCore;

namespace VCEventEase.Models
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Venue> Venue { get; set; }
    }
    
    
}
