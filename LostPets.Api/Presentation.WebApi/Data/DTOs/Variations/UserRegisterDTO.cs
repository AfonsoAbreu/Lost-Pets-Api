using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Data.DTOs.Variations
{
    public class UserRegisterDTO
    {
        [Required]
        public required string userName { get; set; }
        [Required, EmailAddress]
        public required string email { get; set; }
        [Required]
        public required string password { get; set; }
        public ICollection<ContactDTO>? contacts { get; set; }
    }
}
