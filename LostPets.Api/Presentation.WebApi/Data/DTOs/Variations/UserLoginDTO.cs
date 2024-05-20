using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Data.DTOs.Variations
{
    public class UserLoginDTO
    {
        [Required, EmailAddress]
        public required string email { get; set; }
        [Required]
        public required string password { get; set; }
    }
}
