using Infrastructure.Data.Entities;
using Presentation.WebApi.Data.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Data.DTOs
{
    public class ContactDTO : BaseEntityDTO
    {
        [Required]
        public string? content { get; set; }
        [Required]
        public ContactTypeEnum type { get; set; }
    }
}
