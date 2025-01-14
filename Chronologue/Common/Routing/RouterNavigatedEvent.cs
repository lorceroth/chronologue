using Chronologue.Common.Views;
using System;

namespace Chronologue.Common.Routing;

public delegate void RouterNavigatedEventHandler(object sender, RouterNavigatedEventArgs e);

public class RouterNavigatedEventArgs : EventArgs
{
    public RouterNavigatedEventArgs(ViewModelBase page, RouterParameters? parameters = default)
    {
        Page = page;
        Parameters = parameters ?? [];
    }

    public ViewModelBase Page { get; }

    public RouterParameters Parameters { get; }
}
