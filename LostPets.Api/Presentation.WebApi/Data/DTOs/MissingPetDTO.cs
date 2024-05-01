using Infrastructure.Data.Entities;
using Presentation.WebApi.Data.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Data.DTOs
{
    public class MissingPetDTO : BaseEntityDTO
    {
        [Required, MinLength(1)]
        public ICollection<SightingDTO>? sightings { get; set; }
        public Guid? userId { get; set; }
        [Required]
        public PetDTO? pet { get; set; }
        public ICollection<CommentDTO>? comments { get; set; }
        [Required]
        public MissingPetStatusEnum? status { get; set; }
    }
}
