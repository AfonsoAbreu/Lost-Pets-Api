using Infrastructure.Data.Entities;
using Infrastructure.Data.EntityConfigs.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigs
{
    public class SightingConfig : BaseEntityConfig<Sighting>
    {
        public override void Configure(EntityTypeBuilder<Sighting> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(e => e.User)
                .WithMany(e => e.Sightings)
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder
                .HasOne(e => e.MissingPet)
                .WithMany(e => e.Sightings)
                .HasForeignKey(e => e.MissingPetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
