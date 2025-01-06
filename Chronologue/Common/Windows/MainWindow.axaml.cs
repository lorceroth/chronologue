using Avalonia.Controls;

namespace Chronologue.Common.Windows;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Position = new(Screens.Primary!.Bounds.Width - (int)Width - 40, 40);
    }
}
