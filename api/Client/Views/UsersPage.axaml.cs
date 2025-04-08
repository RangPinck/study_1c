using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.ViewModels;

namespace Client;

public partial class UsersPage : UserControl
{
    public UsersPage()
    {
        InitializeComponent();

        DataContext = new UsersPageViewModel();
    }
}