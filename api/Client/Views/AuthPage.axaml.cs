using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.ViewModels;

namespace Client;

public partial class AuthPage : UserControl
{
    public AuthPage()
    {
        InitializeComponent();

        DataContext = new AuthPageViewModel();
    }
}