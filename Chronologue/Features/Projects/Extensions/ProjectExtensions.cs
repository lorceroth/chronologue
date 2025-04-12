using Chronologue.Features.Projects.Entities;
using Chronologue.Features.Projects.Models;
using Chronologue.Features.Tasks.Entities;
using Chronologue.Features.Tasks.Models;
using System;
using System.Linq;

namespace Chronologue.Features.Projects.Extensions;

public static class ProjectExtensions
{
    public static ProjectDetails ToProjectDetails(this Project? project) => new ProjectDetails
    {
        Id = project.Id,
        Name = project.Name,
        Description = project.Description,
        ItemsCount = project.Tasks.Count,
        TotalHours = project.Tasks.Sum(x => x.HoursSpent),
        WorkedPeriodStart = project.Tasks
            .OrderBy(x => x.Due)
            .FirstOrDefault()
            ?.Due,
        WorkedPeriodEnd = project.Tasks
            .OrderByDescending(x => x.Due)
            .FirstOrDefault()
            ?.Due,
        ItemGroups = [.. project.Tasks
            .OrderByDescending(x => x.Due)
            .GroupBy(x => x.Due.Month)
            .Select((x, i) => new ProjectItemGroup
            {
                IsFirstGroup = i is 0,
                Month = x.GetProjectGroupMonth(),
                Items = [.. x.Select(y => new ListableItem
                {
                    Id = y.Id,
                    Title = y.Title,
                    IsCompleted = y.CompletedAt is not null,
                })],
            })],
    };

    public static DateTime? GetProjectGroupMonth(this IGrouping<int, Item> groupedItems)
    {
        var firstItemDate = groupedItems.FirstOrDefault()?.Due;

        return firstItemDate is not null
            ? new DateTime(firstItemDate.Value.Year, firstItemDate.Value.Month, 1)
            : null;
    }
}
