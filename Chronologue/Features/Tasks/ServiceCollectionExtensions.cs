using Chronologue.Features.Tasks.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Chronologue.Features.Tasks;

public static class ServiceCollectionExtensions
{
    public static void AddTasksFeature(this IServiceCollection services)
    {
        services.AddSingleton<TaskListViewModel>();
    }
}
