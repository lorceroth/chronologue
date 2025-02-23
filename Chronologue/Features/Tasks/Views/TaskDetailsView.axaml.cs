using Avalonia.Controls;

namespace Chronologue.Features.Tasks.Views;

public partial class TaskDetailsView : UserControl
{
    public TaskDetailsView()
    {
        InitializeComponent();
    }

    protected TaskDetailsViewModel ViewModel => (TaskDetailsViewModel)DataContext!;

    private void OnNewNoteTitleKeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key is Avalonia.Input.Key.Enter)
        {
            var text = NewNoteText.Text!;

            NewNoteText.Text = "";

            ViewModel.AddNoteCommand.Execute(text);
        }
    }
}
