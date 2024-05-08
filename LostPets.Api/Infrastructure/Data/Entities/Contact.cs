using Infrastructure.Data.Entities.Base;

namespace Infrastructure.Data.Entities
{
    public enum ContactTypeEnum
    {
        EMAIL,
        PHONE,
        WHATSAPP,
        URL
    }

    public class Contact : BaseEntity
    {
        public ContactTypeEnum Type { get; set; }
        public required string Content { get; set; }

        public Guid UserId { get; set; }
        public required User User { get; set; }
    }
}
