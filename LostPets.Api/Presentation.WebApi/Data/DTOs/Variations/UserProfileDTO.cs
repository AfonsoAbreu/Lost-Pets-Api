using Presentation.WebApi.Data.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Data.DTOs.Variations
{
    public class UserProfileDTO : BaseEntityDTO
    {
        [Required, EmailAddress]
        public string? email { get; set; }
        public ICollection<SightingDTO>? sightings { get; set; }
        public ICollection<CommentDTO>? comments { get; set; }
        public ICollection<MissingPetDTO>? missingPets { get; set; }
    }
}
