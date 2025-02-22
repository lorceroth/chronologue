using Chronologue.Features.Tasks.Entities;
using Chronologue.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chronologue.Features.Tasks.Queries;

public class GetTasksByDayQuery : IRequest<ICollection<Item>>
{
    public GetTasksByDayQuery(DateTime day)
    {
        Day = day;
    }

    public DateTime Day { get; }
}

public class GetTasksByDayQueryHandler : IRequestHandler<GetTasksByDayQuery, ICollection<Item>>
{
    private readonly ApplicationContext _context;

    public GetTasksByDayQueryHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Item>> Handle(GetTasksByDayQuery request, CancellationToken cancellationToken) =>
        await _context.Items
            .Include(x => x.Project)
            .Where(x => x.Due.Date == request.Day.Date)
            .ToListAsync(cancellationToken);
}
