using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        public DbSet<CatTag> CatTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatTag>().HasOne(m => m.Cat).WithMany(m => m.CatTags).HasForeignKey(m => m.CatId);
            modelBuilder.Entity<CatTag>().HasOne(m => m.Tag).WithMany(m => m.CatTags).HasForeignKey(m => m.TagId);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<IEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        break;
                }
            }
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
