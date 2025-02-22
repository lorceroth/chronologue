using Chronologue.Features.Tasks.Entities;
using Chronologue.Features.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronologue.Features.Tasks;

public static class TaskDesignMock
{
    private static List<Item> _items = [
        new Item
        {
            Id = Guid.Parse("e495b0ac-4b0b-4a6c-9f9f-e2f30bdfcb07"),
            Title = "Project A item #1",
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce et egestas ligula.",
            Due = DateTime.UtcNow.Date,
            Project = new()
            {
                Name = "Project A",
            },
            Tags = [
                new()
                {
                    Name = "Feature",
                    Color = "Orange",
                },
                new()
                {
                    Name = "Troubleshooting",
                    Color = "Salmon",
                },
            ],
            Notes = [
                new()
                {
                    Text = "Morbi consectetur, lorem non faucibus consequat, eros nibh mollis ex, ut commodo justo mi eget nunc.",
                    CreatedAt = DateTime.UtcNow.AddHours(-1),
                },
                new()
                {
                    Text = "Etiam vitae pharetra ex. Sed accumsan euismod leo.",
                    CreatedAt = DateTime.UtcNow.AddHours(-2),
                },
            ],
        },
        new Item
        {
            Id = Guid.Parse("7d3d643d-e12f-494b-a6b7-7534a31eedc8"),
            Title = "Project A item #2",
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce et egestas ligula.",
            Due = DateTime.UtcNow.Date,
            Project = new()
            {
                Name = "Project A",
            },
            Tags = [
                new()
                {
                    Name = "Feature",
                    Color = "Orange",
                },
            ],
        },
        new Item
        {
            Id = Guid.Parse("6607492a-ec61-44e6-90da-15cd147ea9c4"),
            Title = "Project A item #3",
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce et egestas ligula.",
            Due = DateTime.UtcNow.Date,
            Project = new()
            {
                Name = "Project A",
            },
            Tags = [
                new()
                {
                    Name = "Troubleshooting",
                    Color = "Salmon",
                },
            ],
            Notes = [
                new()
                {
                    Text = "Etiam vitae pharetra ex. Sed accumsan euismod leo.",
                    CreatedAt = DateTime.UtcNow.AddHours(-2),
                },
            ],
        },
    ];

    private static List<ProjectItemCollection> _projectItems = [
        new ProjectItemCollection
        {
            ProjectName = "Project A",
            Date = DateTime.UtcNow.Date,
            Items = [.. _items!.Select(x => new ListableItem
            {
                Id = x.Id,
                Title = x.Title,
            })],
        }
    ];

    public static List<ProjectItemCollection> GetProjectItems(DateTime date) => [.. _projectItems
        .Where(x => x.Date.Date == date)];

    public static Item? GetItemById(Guid id) => _items.FirstOrDefault(x => x.Id == id);

    public static void AddItem(Item item)
    {
        _items.Add(item);

        _projectItems.First().Items.Add(new ListableItem
        {
            Id = item.Id,
            Title = item.Title,
        });
    }
}
