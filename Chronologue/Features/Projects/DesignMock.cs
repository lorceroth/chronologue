using Chronologue.Features.Projects.Entities;
using Chronologue.Features.Projects.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronologue.Features.Projects;

public static class DesignMock
{
    private static List<Project> _projects = [
        new()
        {
            Id = Guid.Parse("e1c9891e-0543-4212-b29f-a60657d0bf5e"),
            Name = "Project A",
            Description = "A short description about Project A.",
            Color = "#9537c4",
            Tasks = [
                new()
                {
                    Title = "Task #1",
                    Due = DateTime.Parse("2025-01-01"),
                    HoursSpent = 1.5m,
                    UpdatedAt = DateTime.Parse("2025-01-01"),
                },
                new()
                {
                    Title = "Task #2",
                    Due = DateTime.Parse("2025-02-15"),
                    HoursSpent = 2m,
                    UpdatedAt = DateTime.Parse("2025-02-15"),
                },
                new()
                {
                    Title = "Task #3",
                    Due = DateTime.Parse("2025-02-28"),
                    HoursSpent = 2m,
                    UpdatedAt = DateTime.Parse("2025-02-28"),
                },
            ],
            CreatedAt = DateTime.Parse("2025-01-01"),
            UpdatedAt = DateTime.Parse("2025-01-31"),
        },
        new()
        {
            Id = Guid.Parse("f607ec2e-48bd-443f-9779-57c84c3b0975"),
            Name = "Project B",
            Description = "A short description about Project B.",
            Color = "#ad482a",
            Tasks = [
                new()
                {
                    Title = "Task #1",
                    Due = DateTime.Parse("2025-01-02"),
                    UpdatedAt = DateTime.Parse("2025-01-02"),
                },
                new()
                {
                    Title = "Task #2",
                    Due = DateTime.Parse("2025-02-27"),
                    UpdatedAt = DateTime.Parse("2025-02-27"),
                },
            ],
            CreatedAt = DateTime.Parse("2025-01-02"),
            UpdatedAt = DateTime.Parse("2025-01-30"),
        },
        new()
        {
            Id = Guid.Parse("a93a75c1-7ad0-4d52-89d6-73f55b3e9fcc"),
            Name = "Project C",
            Description = "A short description about Project C.",
            Tasks = [
                new()
                {
                    Title = "Task #1",
                    Due = DateTime.Parse("2025-01-03"),
                    UpdatedAt = DateTime.Parse("2025-01-03"),
                },
                new()
                {
                    Title = "Task #2",
                    Due = DateTime.Parse("2025-03-01"),
                    UpdatedAt = DateTime.Parse("2025-03-01"),
                },
            ],
            CreatedAt = DateTime.Parse("2025-01-03"),
            UpdatedAt = DateTime.Parse("2025-01-29"),
        },
    ];

    public static List<Project> GetAllProjects() => _projects;

    public static Project? GetProjectById(Guid id) => _projects.FirstOrDefault(x => x.Id == id);

    public static void AddProject(Project project)
    {
        _projects.Add(project);
    }
}
