using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.ViewModels;

namespace Client;

public partial class BlockPage : UserControl
{
    public BlockPage()
    {
        InitializeComponent();

        DataContext = new BlockPageViewModel();
    }
}