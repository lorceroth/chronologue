using Chronologue.Common.Windows;
using Chronologue.Features.Options.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Chronologue.Features.Options;

public static class ServiceCollectionExtensions
{
    public static void AddOptionsFeature(this IServiceCollection services)
    {
        services.Decorate<MainWindowViewModel>((window, provider) =>
        {
            window.RegisterSidebarItem<OptionsViewModel>('\uf6aa'.ToString(), "Options", Constants.Context);

            return window;
        });

        services.AddSingleton<OptionsViewModel>();
    }
}
