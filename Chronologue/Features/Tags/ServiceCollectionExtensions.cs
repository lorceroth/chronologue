using Chronologue.Features.Tags.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Chronologue.Features.Tags;

public static class ServiceCollectionExtensions
{
    public static void AddTagsFeature(this IServiceCollection services)
    {
        services.AddSingleton<TagListViewModel>();
    }
}
