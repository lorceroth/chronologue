using Avalonia;
using Chronologue.Extensions;
using Chronologue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace Chronologue;

internal sealed class Program : IDesignTimeDbContextFactory<ApplicationContext>
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) =>
        BuildAvaloniaApp(CreateServiceCollection().BuildServiceProvider())
            .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp(IServiceProvider services)
        => AppBuilder.Configure(() => new App(services))
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

    public ApplicationContext CreateDbContext(string[] args)
    {
        var services = CreateServiceCollection().BuildServiceProvider();

        return new ApplicationContext(services.GetRequiredService<DbContextOptions>());
    }

    private static IServiceCollection CreateServiceCollection()
    {
        var services = new ServiceCollection();
        services.RegisterApplicationSettings();
        services.RegisterServices();

        return services;
    }
}
