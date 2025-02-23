using Chronologue.Features.Tasks.Entities;
using Chronologue.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chronologue.Features.Tasks.Commands;

public class AddTaskNoteCommand : IRequest<ItemNote?>
{
    public AddTaskNoteCommand(Guid itemId, string? text)
    {
        ItemId = itemId;
        Text = text;
    }

    public Guid ItemId { get; }

    public string? Text { get; }
}

public class AddTaskNoteCommandHandler : IRequestHandler<AddTaskNoteCommand, ItemNote?>
{
    private readonly ApplicationContext _context;

    public AddTaskNoteCommandHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<ItemNote?> Handle(AddTaskNoteCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.Items
            .Include(x => x.Notes)
            .FirstOrDefaultAsync(x => x.Id == request.ItemId);

        if (item is null)
        {
            return default;
        }

        var note = new ItemNote
        {
            Text = request.Text,
        };

        item.Notes.Add(note);

        await _context.SaveChangesAsync(cancellationToken);

        return note;
    }
}
