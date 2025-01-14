using System;

namespace Chronologue.Features.Tasks.Entities;

public class Note
{
    public Guid Id { get; set; }

    public string? Text { get; set; }

    public DateTime PostedAt { get; set; }
}
