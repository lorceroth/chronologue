using Chronologue.Features.Tasks.Entities;
using Chronologue.Infrastructure.Persistence;
using System;
using System.Collections.Generic;

namespace Chronologue.Features.Tags.Entities;

public class Tag : IEntity
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Color { get; set; }

    public ICollection<Item> Items { get; set; } = [];
}
