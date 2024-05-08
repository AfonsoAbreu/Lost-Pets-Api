using Infrastructure.Data.Entities;
using Infrastructure.Data.EntityConfigs.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigs
{
    public class ContactConfig : BaseEntityConfig<Contact>
    {
        public override void Configure(EntityTypeBuilder<Contact> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(e => e.User)
                .WithMany(e => e.Contacts)
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
