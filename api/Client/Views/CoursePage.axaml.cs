using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.ViewModels;

namespace Client;

public partial class CoursePage : UserControl
{
    public CoursePage()
    {
        InitializeComponent();

        DataContext = new CoursePageViewModel();
    }
}