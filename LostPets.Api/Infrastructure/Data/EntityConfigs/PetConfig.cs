using Infrastructure.Data.Entities;
using Infrastructure.Data.EntityConfigs.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigs
{
    public class PetConfig : BaseEntityConfig<Pet>
    {
        public override void Configure(EntityTypeBuilder<Pet> builder)
        {
            base.Configure(builder);
        }
    }
}
