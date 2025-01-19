using System;

namespace Chronologue.Infrastructure.Persistence;

public interface ITimestampedEntity
{
    DateTime CreatedAt { get; set; }

    DateTime? UpdatedAt { get; set; }
}
