﻿using Chronologue.Common.Windows;
using Chronologue.Features.Tags.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Chronologue.Features.Tags;

public static class ServiceCollectionExtensions
{
    public static void AddTagsFeature(this IServiceCollection services)
    {
        services.Decorate<MainWindowViewModel>((window, provider) =>
        {
            window.RegisterSidebarItem<TagListViewModel>('\uf77d'.ToString(), "Tags", Constants.Context);

            return window;
        });

        services.AddSingleton<TagListViewModel>();
    }
}
