using Infrastructure.Data.Entities.Base;

namespace Infrastructure.Data.Entities
{
    public class Image : BaseEntity
    {
        public required string Location { get; set; }

        public User? User { get; set; }
    }
}
