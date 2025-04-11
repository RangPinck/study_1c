using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ReactiveUI;
using Client.Models;
using Client.Models.Courses;

namespace Client.ViewModels
{
	public class CoursePageViewModel : ViewModelBase
	{

		private List<ShortCourseDTO> _courses = new();

        public List<ShortCourseDTO> Courses { get => _courses; set => this.RaiseAndSetIfChanged(ref _courses, value); }
       

        public CoursePageViewModel()
		{
            GetCourses();
        }

        public void ToAddCourse()
        {
            MainWindowViewModel.Instance.PageContent = new AddEditCourse();
        }

        async Task GetCourses()
        {
            var response = await MainWindowViewModel.ApiClient.GetAllCourses(MainWindowViewModel.Instance.CurrentUser.Token);
            Courses = JsonConvert.DeserializeObject<List<ShortCourseDTO>>(response);
        }
    }
}