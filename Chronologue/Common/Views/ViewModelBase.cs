using CommunityToolkit.Mvvm.ComponentModel;

namespace Chronologue.Common.Views;

public class ViewModelBase : ObservableObject
{
    public virtual string? Context { get; }

    public virtual void Initialize() { }
}
