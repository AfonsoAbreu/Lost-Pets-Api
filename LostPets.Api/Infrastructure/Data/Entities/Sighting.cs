using Infrastructure.Data.Entities.Base;
using NetTopologySuite.Geometries;

namespace Infrastructure.Data.Entities
{
    public class Sighting : BaseEntity
    {
        public DateTime SightingDate { get; set; }
        public required Point Location { get; set; }
        public string? Description { get; set; }

        public Guid UserId { get; set; }
        public required User User { get; set; }

        public Guid MissingPetId { get; set; }
        public required MissingPet MissingPet { get; set; }
    }
}
