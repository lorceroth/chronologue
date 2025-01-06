using Chronologue.Common.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Chronologue.Common.Routing;

public class Router
{
    public event NavigatedEventHandler Navigated;

    private readonly IServiceProvider _services;

    public Router(IServiceProvider services)
    {
        _services = services;
    }

    public void Navigate<T>() where T : ViewModelBase => Navigate(typeof(T));

    public void Navigate(Type? pageType)
    {
        if (pageType is null)
        {
            return;
        }

        var page = _services.GetRequiredService(pageType) as ViewModelBase;

        if (page is not null)
        {
            Navigated?.Invoke(this, new(page));
        }
    }
}
