using Infrastructure.Data.Entities;
using Infrastructure.Data.EntityConfigs.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigs
{
    public class ImageConfig : BaseEntityConfig<Image>
    {
        public override void Configure(EntityTypeBuilder<Image> builder)
        {
            base.Configure(builder);
        }
    }
}
