using Chronologue.Features.Tasks.Entities;
using Chronologue.Infrastructure.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chronologue.Features.Tasks.Commands;

public class CreateTaskCommand : IRequest<Item>
{
    public CreateTaskCommand(string title, DateTime due)
    {
        Title = title;
        Due = due;
    }

    public string Title { get; }

    public DateTime Due { get; }
}

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Item>
{
    private readonly ApplicationContext _context;

    public CreateTaskCommandHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Item> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var item = new Item
        {
            Title = request.Title,
            Due = request.Due,
        };

        _context.Items.Add(item);

        await _context.SaveChangesAsync(cancellationToken);

        return item;
    }
}
