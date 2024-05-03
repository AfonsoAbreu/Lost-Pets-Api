using Presentation.WebApi.Data.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Data.DTOs.Variations
{
    public class CommentDTOWithRequiredMissingPetId : BaseEntityDTO
    {
        public Guid? userId { get; set; }
        public Guid? awnsersTo { get; set; }
        [Required]
        public Guid? missingPetId { get; set; }
        public ICollection<CommentDTO>? awnsers { get; set; }
        [Required]
        public string? content { get; set; }
    }
}
