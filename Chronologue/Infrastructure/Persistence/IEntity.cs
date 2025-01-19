using System;

namespace Chronologue.Infrastructure.Persistence;

public interface IEntity
{
    public Guid Id { get; set; }
}
