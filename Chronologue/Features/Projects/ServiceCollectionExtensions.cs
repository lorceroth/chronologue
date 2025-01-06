using Chronologue.Features.Projects.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Chronologue.Features.Projects;

public static class ServiceCollectionExtensions
{
    public static void AddProjectsFeature(this IServiceCollection services)
    {
        services.AddSingleton<ProjectListViewModel>();
    }
}
