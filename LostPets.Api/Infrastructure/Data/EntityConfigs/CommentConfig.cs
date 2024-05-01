using Infrastructure.Data.Entities;
using Infrastructure.Data.EntityConfigs.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigs
{
    public class CommentConfig : BaseEntityConfig<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(e => e.AwnsersToComment)
                .WithMany(e => e.Awnsers)
                .HasForeignKey(e => e.AwnsersTo)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .HasOne(e => e.User)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder
                .HasOne(e => e.MissingPet)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.MissingPetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
