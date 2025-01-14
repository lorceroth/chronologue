using Chronologue.Features.Projects.Entities;
using System;
using System.Collections.Generic;

namespace Chronologue.Features.Tasks.Models;

public class ProjectItemCollection
{
    public Project? Project { get; set; }

    public DateTime Date { get; set; }

    public ICollection<ListableItem> Items { get; set; } = [];
}
