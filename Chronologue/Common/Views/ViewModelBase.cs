using Avalonia.Controls;
using Chronologue.Common.Routing;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chronologue.Common.Views;

public class ViewModelBase : ObservableObject
{
    public virtual string? Context { get; }

    public ViewModelBase()
    {
        if (Design.IsDesignMode)
        {
            DesignInitialize();
        }
    }

    public virtual void DesignInitialize() { }

    public virtual void Initialize() { }

    public virtual void Navigated(RouterParameters parameters) { }
}
