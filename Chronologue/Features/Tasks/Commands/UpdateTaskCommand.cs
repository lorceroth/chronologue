using Chronologue.Features.Tags.Entities;
using Chronologue.Features.Tasks.Entities;
using Chronologue.Infrastructure.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Chronologue.Features.Tasks.Commands;

public class UpdateTaskCommand : IRequest<Item?>
{
    public UpdateTaskCommand(Guid id, string? title, string? description, DateTime due, Guid? projectId, ICollection<Tag> tags, decimal hoursSpent)
    {
        Id = id;
        Title = title;
        Description = description;
        Due = due;
        ProjectId = projectId;
        Tags = tags;
        HoursSpent = hoursSpent;
    }

    public Guid Id { get; }

    public string? Title { get; }

    public string? Description { get; }

    public DateTime Due { get; }

    public Guid? ProjectId { get; }

    public ICollection<Tag> Tags { get; } = [];

    public decimal HoursSpent { get; }
}

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Item?>
{
    private readonly ApplicationContext _context;

    public UpdateTaskCommandHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Item?> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Items.FindAsync(request.Id);

        if (item is null)
        {
            return default;
        }

        item.Title = request.Title;
        item.Description = request.Description;
        item.Due = request.Due;
        item.ProjectId = request.ProjectId;
        item.Tags = request.Tags;

        await _context.SaveChangesAsync(cancellationToken);

        return item;
    }
}
