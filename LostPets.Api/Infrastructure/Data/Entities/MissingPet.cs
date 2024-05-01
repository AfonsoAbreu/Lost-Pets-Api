using Infrastructure.Data.Entities.Base;

namespace Infrastructure.Data.Entities
{
    public enum MissingPetStatusEnum
    {
        LOST = 0,
        FOUND = 1,
        DEACTIVATED = 2
    }

    public class MissingPet : BaseEntity
    {
        public MissingPetStatusEnum Status { get; set; }

        public Guid UserId { get; set; }
        public required User User { get; set; }

        public Guid PetId { get; set; }
        public required Pet Pet { get; set; }

        public required ICollection<Sighting> Sightings { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
