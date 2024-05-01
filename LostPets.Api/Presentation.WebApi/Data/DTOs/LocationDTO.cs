using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Data.DTOs
{
    public class LocationDTO
    {
        [Required, Range(-90, 90)]
        public double latitude { get; set; }
        [Required, Range(-180, 180)]
        public double longitude { get; set; }
    }
}
