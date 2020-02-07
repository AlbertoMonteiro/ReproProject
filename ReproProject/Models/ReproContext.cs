using Microsoft.EntityFrameworkCore;

namespace ReproProject.Models
{
    public class ReproContext : DbContext
    {
        public ReproContext(DbContextOptions<ReproContext> options)
            :base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventDateDetails> EventDatesDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
