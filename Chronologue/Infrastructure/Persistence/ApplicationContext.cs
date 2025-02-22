using Chronologue.Features.Projects.Entities;
using Chronologue.Features.Tags.Entities;
using Chronologue.Features.Tasks.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chronologue.Infrastructure.Persistence;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }

    public DbSet<ItemNote> ItemNotes { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public override int SaveChanges()
    {
        SetTimestamps();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTimestamps();

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ConfigureEntities(builder);
    }

    private void SetTimestamps()
    {
        foreach (var (state, entity) in ChangeTracker.Entries()
            .Where(x => x.Entity is ITimestampedEntity)
            .Select(x => (x.State, x.Entity as ITimestampedEntity)))
        {
            switch (state)
            {
                case EntityState.Added:
                    entity!.CreatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entity!.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }

    private void ConfigureEntities(ModelBuilder builder)
    {
        builder.Entity<Item>(entity =>
        {
            entity.Property(x => x.Title).IsRequired();
            entity.Property(x => x.Due).IsRequired();
        });

        builder.Entity<ItemNote>(entity =>
        {
            entity.Property(x => x.Text).IsRequired();
        });

        builder.Entity<Project>(entity =>
        {
            entity.Property(x => x.Name).IsRequired();
        });

        builder.Entity<Tag>(entity =>
        {
            entity.Property(x => x.Name).IsRequired();
        });
    }
}
