using Presentation.WebApi.Data.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Data.DTOs
{
    public class UserDTO : BaseEntityDTO
    {
        [Required, EmailAddress]
        public string? email { get; set; }
        public string? userName { get; set; }
        public ImageDTO? image { get; set; }
        public ICollection<ContactDTO>? contacts { get; set; }
    }
}
