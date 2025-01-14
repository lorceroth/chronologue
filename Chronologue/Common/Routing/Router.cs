using Chronologue.Common.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Chronologue.Common.Routing;

public class Router
{
    public event RouterNavigatedEventHandler? Navigated;

    private readonly IServiceProvider _services;

    public Router(IServiceProvider services)
    {
        _services = services;
    }

    public void Navigate<T>(RouterParameters? parameters = default) where T : ViewModelBase =>
        Navigate(typeof(T), parameters);

    public void Navigate(Type? pageType, RouterParameters? parameters = default)
    {
        if (pageType is null)
        {
            return;
        }

        var page = _services.GetRequiredService(pageType) as ViewModelBase;

        if (page is not null)
        {
            Navigated?.Invoke(this, new(page, parameters));
        }
    }
}
