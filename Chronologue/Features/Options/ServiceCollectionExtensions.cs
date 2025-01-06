using Chronologue.Features.Options.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Chronologue.Features.Options;

public static class ServiceCollectionExtensions
{
    public static void AddOptionsFeature(this IServiceCollection services)
    {
        services.AddSingleton<OptionsViewModel>();
    }
}
