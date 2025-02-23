using Chronologue.Features.Tasks.Entities;
using Chronologue.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chronologue.Features.Tasks.Queries;

public class GetTaskByIdQuery : IRequest<Item?>
{
    public GetTaskByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, Item?>
{
    private readonly ApplicationContext _context;

    public GetTaskByIdQueryHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Item?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _context.Items
            .Include(x => x.Notes)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (item is null)
        {
            return default;
        }

        return item;
    }
}
