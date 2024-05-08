using Infrastructure.Data.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Entities
{
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<MissingPet>? MissingPets { get; set; }
        public ICollection<Sighting>? Sightings { get; set; }
        public ICollection<Contact>? Contacts { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
