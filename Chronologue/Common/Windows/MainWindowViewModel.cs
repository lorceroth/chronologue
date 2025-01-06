using Chronologue.Common.Views;
using Chronologue.Features.Tasks.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chronologue.Common.Windows;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _page;

    public MainWindowViewModel()
    {
        Page = new TaskListViewModel();
    }
}
