using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Models.Courses;
using Client.Models.Users;
using Newtonsoft.Json;
using ReactiveUI;

namespace Client.ViewModels
{
	public class AddEditCourseViewModel : ViewModelBase
	{

		private AddCourseDTO _courseFull = new();

        private List<CourseAuthorDTO> _courseAutor = new();

        private int _authorIndex = 0;

        bool isEdit = false;


        public AddEditCourseViewModel() {

            CourseFull = new AddCourseDTO();
            GetAuthors();
        }

        public AddCourseDTO CourseFull { get => _courseFull; set => this.RaiseAndSetIfChanged (ref _courseFull, value); }
        public List<CourseAuthorDTO> CourseAutor { get => _courseAutor; set => this.RaiseAndSetIfChanged(ref _courseAutor, value); }
        public int AuthorIndex { get => _authorIndex; set => this.RaiseAndSetIfChanged(ref _authorIndex, value); }

        async Task GetAuthors()
        {
            var AuthorsResponse = await MainWindowViewModel.ApiClient.GetAuthors();
            CourseAutor = JsonConvert.DeserializeObject<List<CourseAuthorDTO>>(AuthorsResponse);
        }

        public async Task SaveData()
        {
            

            string result;

            if (!isEdit)
            {
                CourseFull.Author = CourseAutor[AuthorIndex].UserId;
                await MainWindowViewModel.ApiClient.AddCourse(CourseFull);
                MainWindowViewModel.Instance.PageContent = new CoursePage();
            }
            else
            {

            }
        }
    }
}