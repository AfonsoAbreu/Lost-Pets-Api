﻿namespace Infrastructure.Data.Entities.Base
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
        DateTime? CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
