using Presentation.WebApi.Data.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Data.DTOs
{
    public class PetDTO : BaseEntityDTO
    {
        [Required]
        public string? name { get; set; }
        [Required]
        public string? species { get; set; }
        [Required, Range(0, int.MaxValue)]
        public int? age { get; set; }
        //public ICollection<PhotoDTO> photos { get; set; }
        public string? description { get; set; }
    }
}
