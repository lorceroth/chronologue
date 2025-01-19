using Chronologue.Features.Projects.Entities;
using Chronologue.Features.Tags.Entities;
using Chronologue.Features.Tasks.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Chronologue.Infrastructure.Persistence;

public partial class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }

    public DbSet<ItemNote> ItemNotes { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ConfigureEntities(builder);
        ConfigureEntityDefaults(builder);
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

    private void ConfigureEntityDefaults(ModelBuilder builder)
    {
        foreach (var entity in builder.Model.FindEntityTypes(typeof(IEntity)))
        {
            var id = entity.FindProperty(nameof(IEntity.Id))!;

            id.IsNullable = false;
            id.ValueGenerated = ValueGenerated.OnAdd;
        }

        foreach (var entity in builder.Model.FindEntityTypes(typeof(ITimestampedEntity)))
        {
            var createdAt = entity.FindProperty(nameof(ITimestampedEntity.CreatedAt))!;
            var updatedAt = entity.FindProperty(nameof(ITimestampedEntity.UpdatedAt))!;

            createdAt.IsNullable = false;
            createdAt.ValueGenerated = ValueGenerated.OnAdd;
            updatedAt.ValueGenerated = ValueGenerated.OnUpdate;
        }
    }
}
