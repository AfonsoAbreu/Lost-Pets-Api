using Infrastructure.Data.Entities;
using Presentation.WebApi.Data.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Data.DTOs.Variations
{
    public class MissingPetDTOWithOptionalSightings : BaseEntityDTO
    {
        public ICollection<SightingDTO>? sightings { get; set; }
        public UserDTO? user { get; set; }
        [Required]
        public PetDTO? pet { get; set; }
        public ICollection<CommentDTO>? comments { get; set; }
        [Required]
        public MissingPetStatusEnum? status { get; set; }
    }
}
