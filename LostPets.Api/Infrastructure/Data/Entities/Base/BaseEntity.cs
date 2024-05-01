namespace Infrastructure.Data.Entities.Base
{
    public abstract class BaseEntity : IEquatable<BaseEntity?>
    {
        public BaseEntity() 
        { 
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BaseEntity);
        }

        public bool Equals(BaseEntity? other)
        {
            return other is not null &&
                   Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
