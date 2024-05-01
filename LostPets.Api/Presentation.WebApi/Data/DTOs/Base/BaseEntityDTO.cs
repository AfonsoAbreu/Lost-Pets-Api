namespace Presentation.WebApi.Data.DTOs.Base
{
    public abstract class BaseEntityDTO
    {
        public Guid? id { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
    }
}
