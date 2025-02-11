using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using StealAllTheCatsAssignment.Models;

namespace StealAllTheCatsAssignment.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Cat> Cats { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cat>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Cats)
                .UsingEntity<CatTags>(
                    j => j.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP"));
        }


    }
}
