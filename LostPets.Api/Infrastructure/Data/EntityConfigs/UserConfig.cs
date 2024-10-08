using Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(e => e.CreatedAt)
                .HasDefaultValueSql("getdate()");

            builder
                .Navigation(e => e.Contacts)
                .AutoInclude();

            builder
                .Navigation(e => e.Image)
                .AutoInclude();

            builder
                .HasOne(e => e.Image)
                .WithOne(e => e.User)
                .HasForeignKey<User>(e => e.ImageId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
