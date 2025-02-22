using Chronologue.Common.Routing;
using Chronologue.Common.Windows;
using Chronologue.Features.Options;
using Chronologue.Features.Projects;
using Chronologue.Features.Tags;
using Chronologue.Features.Tasks;
using Chronologue.Infrastructure;
using Chronologue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Chronologue.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterApplicationSettings(this IServiceCollection services)
    {
        var settings = new ApplicationSettings
        {
            ApplicationDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Chronologue"),
        };

        if (Directory.Exists(settings.ApplicationDataPath) is false)
        {
            Directory.CreateDirectory(settings.ApplicationDataPath);
        }

        services.AddSingleton(settings);
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationContext>((provider, config) =>
        {
            var settings = provider.GetRequiredService<ApplicationSettings>();
            var path = Path.Combine(settings.ApplicationDataPath, "Chronologue.sqlite");

            config.UseSqlite($"Data Source={path}");
        });

        services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<Program>());

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<Router>();

        services.AddTasksFeature();
        services.AddProjectsFeature();
        services.AddTagsFeature();
        services.AddOptionsFeature();
    }
}
