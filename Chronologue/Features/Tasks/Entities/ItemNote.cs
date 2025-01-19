using Chronologue.Infrastructure.Persistence;
using System;

namespace Chronologue.Features.Tasks.Entities;

public class ItemNote : IEntity, ITimestampedEntity
{
    public Guid Id { get; set; }

    public string? Text { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
