using Infrastructure.Data.Entities.Base;

namespace Infrastructure.Data.Entities
{
    public class MissingPetImage : BaseEntity
    {
        public Guid MissingPetId { get; set; }
        public required MissingPet MissingPet { get; set; }

        public Guid ImageId { get; set; }
        public required Image Image { get; set; }
    }
}
