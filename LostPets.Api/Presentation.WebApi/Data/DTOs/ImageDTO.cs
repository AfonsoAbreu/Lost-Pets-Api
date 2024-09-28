using Infrastructure.Data.Entities;
using Presentation.WebApi.Data.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Data.DTOs
{
    public class ImageDTO : BaseEntityDTO
    {
        public string url { get; set; }
    }
}
