using Chronologue.Features.Tasks.Entities;
using Chronologue.Infrastructure.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chronologue.Features.Tasks.Commands;

public class DeleteTaskCommand : IRequest<Item?>
{
    public DeleteTaskCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Item?>
{
    private readonly ApplicationContext _context;

    public DeleteTaskCommandHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Item?> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Items.FindAsync(request.Id);

        if (item is null)
        {
            return default;
        }

        _context.Items.Remove(item);

        await _context.SaveChangesAsync(cancellationToken);

        return item;
    }
}
