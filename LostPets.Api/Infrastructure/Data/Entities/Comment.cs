using Infrastructure.Data.Entities.Base;

namespace Infrastructure.Data.Entities
{
    public class Comment : BaseEntity
    {
        public required string Content { get; set; }

        public Guid? MissingPetId { get; set; }
        public required MissingPet MissingPet { get; set; }

        public Guid UserId { get; set; }
        public required User User { get; set; }

        public Guid? AwnsersTo { get; set; }
        public Comment? AwnsersToComment { get; set; }

        public ICollection<Comment>? Awnsers { get; set; }
    }
}