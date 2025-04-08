using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.ViewModels;

namespace Client;

public partial class UserStatistics : UserControl
{
    public UserStatistics()
    {
        InitializeComponent();

        DataContext = new UserStatisticsViewModel();
    }
}