using Chronologue.Common.Windows;
using Chronologue.Features.Tasks.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Chronologue.Features.Tasks;

public static class ServiceCollectionExtensions
{
    public static void AddTasksFeature(this IServiceCollection services)
    {
        services.Decorate<MainWindowViewModel>((window, provider) =>
        {
            window.RegisterSidebarItem<TaskListViewModel>('\ue35e'.ToString(), "Tasks", Constants.Context);

            return window;
        });

        services.AddSingleton<TaskListViewModel>();
        services.AddSingleton<TaskDetailsViewModel>();
        services.AddSingleton<TaskFormViewModel>();
    }
}
