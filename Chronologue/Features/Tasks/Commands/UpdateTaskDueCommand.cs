using Chronologue.Features.Tasks.Entities;
using Chronologue.Infrastructure.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chronologue.Features.Tasks.Commands;

public class UpdateTaskDueCommand : IRequest<Item?>
{
    public UpdateTaskDueCommand(Guid id, DateTime due)
    {
        Id = id;
        Due = due;
    }

    public Guid Id { get; }

    public DateTime Due { get; }
}

public class UpdateTaskDueCommandHandler : IRequestHandler<UpdateTaskDueCommand, Item?>
{
    private readonly ApplicationContext _context;

    public UpdateTaskDueCommandHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Item?> Handle(UpdateTaskDueCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Items.FindAsync(request.Id);

        if (item is null)
        {
            return default;
        }

        item.Due = request.Due;

        await _context.SaveChangesAsync(cancellationToken);

        return item;
    }
}
