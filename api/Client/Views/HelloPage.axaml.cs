using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.ViewModels;

namespace Client;

public partial class HelloPage : UserControl
{
    public HelloPage()
    {
        InitializeComponent();
        DataContext = new HelloPageViewModel();
    }

    private void Binding(object? sender, Avalonia.Input.TextInputEventArgs e)
    {
    }
}