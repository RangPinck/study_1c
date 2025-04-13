using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.Models.Courses;
using Client.ViewModels;

namespace Client;

public partial class AddEditCourse : UserControl
{
    public AddEditCourse()
    {
        InitializeComponent();

        DataContext = new AddEditCourseViewModel();
    }

    public AddEditCourse(ShortCourseDTO Item)
    {
        InitializeComponent();

        DataContext = new AddEditCourseViewModel(Item);
    }
}