using Events.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Events.API.Data
{
    public class EventsDbContext : DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Un événement peut avoir plusieurs catégories
            modelBuilder.Entity<Evenement>()
                .HasMany(e => e.Categories);

            // Un événement peut avoir plusieurs participations dont chacune correspond à un seul événement
            modelBuilder.Entity<Evenement>()
                .HasMany(e => e.Participations)
                .WithOne(p => p.Evenement)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            CheckForDeletedProperty();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CheckForDeletedProperty();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            CheckForDeletedProperty();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void CheckForDeletedProperty()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                var isDeletedProp = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "isDeleted");
                if (isDeletedProp != null)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            isDeletedProp.CurrentValue = false;
                            break;
                        case EntityState.Deleted:
                            isDeletedProp.CurrentValue = true;
                            entry.State = EntityState.Modified;
                            break;
                    }
                }

            }
        }

        public DbSet<Ville>? Villes { get; set; }
        public DbSet<Evenement>? Evenements { get; set; }
        public DbSet<Categorie>? Categories { get; set; }
        public DbSet<Participation>? Participations { get; set; }
    }
}
