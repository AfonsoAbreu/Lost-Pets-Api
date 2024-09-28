using Infrastructure.Data.Entities.Base;

namespace Infrastructure.Data.Entities
{
    public class Image : BaseEntity
    {
        public string? Location { get; set; }
        public required string Url { get; set; }
        public bool IsLocal { 
            get 
            { 
                return Location != null;
            } 
        }

        public User? User { get; set; }

        public ICollection<MissingPet>? MissingPets { get; set; }
        public ICollection<MissingPetImage>? MissingPetImages { get; set; }
    }
}
