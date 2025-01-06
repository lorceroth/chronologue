using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Chronologue.Common.Windows;
using Chronologue.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Chronologue;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var culture = new CultureInfo("en-US");
        culture.DateTimeFormat.ShortDatePattern = "MMMM d, yyyy";
        culture.DateTimeFormat.LongDatePattern = "yyyy-MM-dd";

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        var services = new ServiceCollection();
        services.RegisterServices();

        var provider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();

            var mainWindowViewModel = provider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.Initialize();

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
