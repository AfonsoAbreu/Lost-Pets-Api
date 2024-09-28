using Infrastructure.Data.Entities;
using Infrastructure.Data.EntityConfigs.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigs
{
    public class MissingPetImageConfig : BaseEntityConfig<MissingPetImage>
    {
        public override void Configure(EntityTypeBuilder<MissingPetImage> builder)
        {
            base.Configure(builder);
        }
    }
}
