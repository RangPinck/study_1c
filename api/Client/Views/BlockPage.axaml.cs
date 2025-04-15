using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.Models.Courses;
using Client.ViewModels;

namespace Client;

public partial class BlockPage : UserControl
{
    public BlockPage()
    {
        InitializeComponent();

        DataContext = new BlockPageViewModel();
    }

    public BlockPage(ShortCourseDTO currentCourse)
    {
        InitializeComponent();

        DataContext = new BlockPageViewModel(currentCourse);
    }
}