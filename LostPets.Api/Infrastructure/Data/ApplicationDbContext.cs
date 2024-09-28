using Infrastructure.Data.Entities;
using Infrastructure.Data.Entities.Base;
using Infrastructure.Data.EntityConfigs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {

        #region Entities

            public DbSet<Pet> Pets { get; set; }
            public DbSet<MissingPet> MissingPets { get; set; }
            public DbSet<Comment> Comments { get; set; }
            public DbSet<Sighting> Sightings { get; set; }

        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Entity Configs
            base.OnModelCreating(builder);

            new UserConfig().Configure(builder.Entity<User>());
            new PetConfig().Configure(builder.Entity<Pet>());
            new MissingPetConfig().Configure(builder.Entity<MissingPet>());
            new CommentConfig().Configure(builder.Entity<Comment>());
            new SightingConfig().Configure(builder.Entity<Sighting>());
            new ImageConfig().Configure(builder.Entity<Image>());
            new MissingPetImageConfig().Configure(builder.Entity<MissingPetImage>());
            #endregion
        }

        #region SaveChanges overrides

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        #endregion

        private void AddTimestamps()
        {
            var entities =
                from entry in ChangeTracker.Entries()
                let entity = entry.Entity
                where entity is IBaseEntity
                    && entry.State == EntityState.Modified
                select entity;

            foreach (var entity in entities)
            {
                DateTime now = DateTime.UtcNow;

                ((IBaseEntity)entity).UpdatedAt = now;
            }
        }

    }
}
