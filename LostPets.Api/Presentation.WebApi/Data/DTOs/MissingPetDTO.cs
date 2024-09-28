using Infrastructure.Data.Entities;
using Presentation.WebApi.Data.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Data.DTOs
{
    public class MissingPetDTO : BaseEntityDTO
    {
        public UserDTO? user { get; set; }
        [Required]
        public PetDTO? pet { get; set; }
        [Required]
        public MissingPetStatusEnum? status { get; set; }

        [Required, MinLength(1)]
        public ICollection<SightingDTO>? sightings { get; set; }
        public ICollection<ImageDTO>? images { get; set; }
        public ICollection<CommentDTO>? comments { get; set; }
    }
}
