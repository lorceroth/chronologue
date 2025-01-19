using Chronologue.Features.Projects.Entities;
using Chronologue.Features.Tags.Entities;
using Chronologue.Infrastructure.Persistence;
using System;
using System.Collections.Generic;

namespace Chronologue.Features.Tasks.Entities;

public class Item : IEntity, ITimestampedEntity
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime Due { get; set; }

    public Project? Project { get; set; }

    public Guid? ProjectId { get; set; }

    public ICollection<Tag> Tags { get; set; } = [];

    public ICollection<ItemNote> Notes { get; set; } = [];

    public decimal HoursSpent { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsUpToDate => CompletedAt is null && Due >= DateTime.UtcNow.Date;

    public bool IsOverdue => CompletedAt is null && Due < DateTime.UtcNow.Date;
}
