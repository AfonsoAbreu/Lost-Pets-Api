using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<MissingPet>? MissingPets { get; set; }
        public ICollection<Sighting>? Sightings { get; set; }
    }
}
