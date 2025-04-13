using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.ViewModels;

namespace Client;

public partial class AddEditCourse : UserControl
{
    public AddEditCourse()
    {
        InitializeComponent();

        DataContext = new AddEditCourseViewModel();
    }
}