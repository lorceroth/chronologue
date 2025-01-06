using Chronologue.Common.Windows;
using Chronologue.Features.Projects.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Chronologue.Features.Projects;

public static class ServiceCollectionExtensions
{
    public static void AddProjectsFeature(this IServiceCollection services)
    {
        services.Decorate<MainWindowViewModel>((window, provider) =>
        {
            window.RegisterSidebarItem<ProjectListViewModel>('\uf1fd'.ToString(), "Projects", Constants.Context);

            return window;
        });

        services.AddSingleton<ProjectListViewModel>();
    }
}
