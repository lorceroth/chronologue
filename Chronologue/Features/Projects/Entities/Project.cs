using Chronologue.Features.Tasks.Entities;
using Chronologue.Infrastructure.Persistence;
using System;
using System.Collections.Generic;

namespace Chronologue.Features.Projects.Entities;

public class Project : IEntity, ITimestampedEntity
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Color { get; set; }

    public ICollection<Item> Tasks { get; set; } = [];

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
