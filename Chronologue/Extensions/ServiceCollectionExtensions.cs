using Chronologue.Common.Routing;
using Chronologue.Common.Windows;
using Chronologue.Features.Options;
using Chronologue.Features.Projects;
using Chronologue.Features.Tags;
using Chronologue.Features.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Chronologue.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<Router>();

        services.AddTasksFeature();
        services.AddProjectsFeature();
        services.AddTagsFeature();
        services.AddOptionsFeature();
    }
}
