using Infrastructure.Data.Entities;
using Infrastructure.Data.EntityConfigs.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigs
{
    public class MissingPetConfig : BaseEntityConfig<MissingPet>
    {
        public override void Configure(EntityTypeBuilder<MissingPet> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(e => e.User)
                .WithMany(e => e.MissingPets)
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder
                .HasOne(e => e.Pet)
                .WithOne(e => e.MissingPet)
                .HasForeignKey<MissingPet>(e => e.PetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder
                .Navigation(e => e.Pet)
                .AutoInclude();

            builder
                .Navigation(e => e.User)
                .AutoInclude();
        }
    }
}
