using Chronologue.Features.Tasks.Entities;
using Chronologue.Infrastructure.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chronologue.Features.Tasks.Commands;

public class UpdateTaskCompletionCommand : IRequest<Item?>
{
    public UpdateTaskCompletionCommand(Guid id, DateTime? completedAt)
    {
        Id = id;
        CompletedAt = completedAt;
    }

    public Guid Id { get; }

    public DateTime? CompletedAt { get; }
}

public class UpdateTaskCompletionCommandHandler : IRequestHandler<UpdateTaskCompletionCommand, Item?>
{
    private readonly ApplicationContext _context;

    public UpdateTaskCompletionCommandHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Item?> Handle(UpdateTaskCompletionCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Items.FindAsync(request.Id);

        if (item is null)
        {
            return default;
        }

        item.CompletedAt = request.CompletedAt;

        await _context.SaveChangesAsync();

        return item;
    }
}
