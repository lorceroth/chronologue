using Humanizer;

namespace Chronologue.Features.Projects.Models;

public enum ProjectSort
{
    None,
    LastWorkedOn,
    Name,
    Created,
    Updated,
}

public record ProjectSortSelection(ProjectSort Sort, string Icon)
{
    public string Label => Sort.Humanize();
}
