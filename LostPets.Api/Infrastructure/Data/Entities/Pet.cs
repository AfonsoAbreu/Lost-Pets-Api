using Infrastructure.Data.Entities.Base;

namespace Infrastructure.Data.Entities
{
    public class Pet : BaseEntity
    {
        public required string Name { get; set; }
        public required string Species { get; set; }
        public string? Age { get; set; }
        public string? Description { get; set; }

        public required MissingPet MissingPet { get; set; }
        //public ICollection<Photo> Photos { get; set; }
    }
}
