using Chronologue.Common.Routing;
using Chronologue.Common.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Chronologue.Common.Windows;

public record SidebarItem(string Icon, string Text, string Context, Type PageType) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private bool _isActive;

    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;

            PropertyChanged?.Invoke(this, new(nameof(IsActive)));
        }
    }
}

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly Router _router;

    [ObservableProperty]
    private ViewModelBase _page;

    public MainWindowViewModel() { }

    public MainWindowViewModel(Router router)
    {
        _router = router;

        _router.Navigated += OnRouterNavigated;

        NavigateCommand = new RelayCommand<Type>(Navigate);
    }

    public ObservableCollection<SidebarItem> SidebarItems { get; } = new();

    public RelayCommand<Type> NavigateCommand { get; set; }

    public override void Initialize()
    {
        _router.Navigate(SidebarItems.FirstOrDefault()?.PageType);
    }

    public void RegisterSidebarItem<T>(string icon, string text, string context) where T : ViewModelBase
    {
        SidebarItems.Add(new(icon, text, context, typeof(T)));
    }

    public void Navigate(Type? pageType) => _router.Navigate(pageType);

    private void OnRouterNavigated(object sender, RouterNavigatedEventArgs e)
    {
        Page = e.Page;
        Page.Initialize();
        Page.Navigated(e.Parameters);

        UpdateSidebarItemStates();
    }

    private void UpdateSidebarItemStates()
    {
        for (var i = 0; i < SidebarItems.Count; i++)
        {
            SidebarItems[i].IsActive = false;
        }

        var selectedSidebarItem = SidebarItems.FirstOrDefault(x => x.Context == Page.Context);

        if (selectedSidebarItem is not null)
        {
            int index = SidebarItems.IndexOf(selectedSidebarItem);

            SidebarItems[index].IsActive = true;
        }
    }
}
