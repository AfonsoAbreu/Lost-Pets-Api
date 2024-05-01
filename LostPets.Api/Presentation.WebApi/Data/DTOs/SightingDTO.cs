using Presentation.WebApi.Data.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Data.DTOs
{
    public class SightingDTO : BaseEntityDTO
    {
        [Required]
        public DateTime? sightingDate { get; set; }
        [Required]
        public LocationDTO? location { get; set; }
        public Guid? userId { get; set; }
        public Guid? missingPetId { get; set; }
        public string? description { get; set; }
    }
}
