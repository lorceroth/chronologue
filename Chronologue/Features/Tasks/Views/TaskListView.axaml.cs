using Avalonia.Controls;

namespace Chronologue.Features.Tasks.Views;

public partial class TaskListView : UserControl
{
    public TaskListView()
    {
        InitializeComponent();
    }

    protected TaskListViewModel ViewModel => (TaskListViewModel)DataContext!;

    private void OnNewItemTitleKeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key is Avalonia.Input.Key.Enter)
        {
            var title = NewItemTitle.Text!;

            NewItemTitle.Text = "";

            ViewModel.AddItemCommand.Execute((title, NavigateToForm: e.KeyModifiers is Avalonia.Input.KeyModifiers.Control));
        }
    }
}
