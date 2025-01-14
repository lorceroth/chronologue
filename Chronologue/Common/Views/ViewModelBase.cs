using Avalonia.Controls;
using Chronologue.Common.Routing;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace Chronologue.Common.Views;

public class ViewModelBase : ObservableObject
{
    public virtual string? Context { get; }

    public ViewModelBase()
    {
        // TODO: Uncomment when the DB is added
        //if (Design.IsDesignMode)
        //{
            DesignInitialize();
        //}
    }

    public virtual void DesignInitialize() { }

    public virtual void Initialize() { }

    public virtual void Navigated(RouterParameters parameters) { }
}
