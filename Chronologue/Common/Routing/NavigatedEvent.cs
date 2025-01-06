using Chronologue.Common.Views;
using System;

namespace Chronologue.Common.Routing;

public delegate void NavigatedEventHandler(object sender, NavigatedEventArgs e);

public class NavigatedEventArgs : EventArgs
{
    public NavigatedEventArgs(ViewModelBase page) => Page = page;

    public ViewModelBase Page { get; }
}
