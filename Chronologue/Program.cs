using Avalonia;
using Chronologue.Extensions;
using Chronologue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Chronologue;

internal sealed class Program : IDesignTimeDbContextFactory<ApplicationContext>
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure(CreateAvaloniaApp)
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

    public ApplicationContext CreateDbContext(string[] args)
    {
        var services = CreateServiceCollection().BuildServiceProvider();

        return new ApplicationContext(services.GetRequiredService<DbContextOptions>());
    }

    private static App CreateAvaloniaApp() => new App(CreateServiceCollection().BuildServiceProvider());

    private static IServiceCollection CreateServiceCollection()
    {
        var services = new ServiceCollection();
        services.RegisterApplicationSettings();
        services.RegisterServices();

        return services;
    }
}
