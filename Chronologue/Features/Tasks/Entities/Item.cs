using Chronologue.Features.Projects.Entities;
using Chronologue.Features.Tags.Entities;
using System;
using System.Collections.Generic;

namespace Chronologue.Features.Tasks.Entities;

public class Item
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime Due { get; set; }

    public Project? Project { get; set; }

    public Guid? ProjectId { get; set; }

    public ICollection<Tag> Tags { get; set; } = [];

    public ICollection<Note> Notes { get; set; } = [];

    public DateTime? CompletedAt { get; set; }

    public bool IsUpToDate => CompletedAt is null && Due >= DateTime.UtcNow.Date;

    public bool IsOverdue => CompletedAt is null && Due < DateTime.UtcNow.Date;
}
