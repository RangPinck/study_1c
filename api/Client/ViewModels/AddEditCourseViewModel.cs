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


        private int _authorIndex = 0;

        bool isEdit = false;

        private List<UserDTO> _users = new();

        public List<UserDTO> Users { get => _users; set => this.RaiseAndSetIfChanged(ref _users, value); }


        public AddEditCourseViewModel() {

            CourseFull = new AddCourseDTO();
            GetAuthors(MainWindowViewModel.Instance.CurrentUser.Token);
        }


        public AddEditCourseViewModel(ShortCourseDTO Item)
        {  
            CourseUpdate.CourseId = Item.CourseId;
            CourseFull.Title = Item.CourseName;
            CourseFull.Link = Item.Link;
            CourseFull.Description = Item.Description;
            GetAuthors(MainWindowViewModel.Instance.CurrentUser.Token);
            isEdit = true;
        }
        public AddCourseDTO CourseFull { get => _courseFull; set => this.RaiseAndSetIfChanged (ref _courseFull, value); }
        public int AuthorIndex { get => _authorIndex; set => this.RaiseAndSetIfChanged(ref _authorIndex, value); }

        private UpdateCourseDTO _courseUpdate = new();
        public UpdateCourseDTO CourseUpdate { get => _courseUpdate; set => this.RaiseAndSetIfChanged(ref _courseUpdate, value); }
        async Task GetAuthors(string token)
        {
          
            var response = await MainWindowViewModel.ApiClient.GetCourses();

            Users = JsonConvert.DeserializeObject<List<UserDTO>>(response);
            AuthorIndex = 1;
        }

        public async Task SaveData()
        {
            

            string result;

            if (!isEdit)
            {
                CourseFull.Author = Users[AuthorIndex].UserId;
                await MainWindowViewModel.ApiClient.AddCourse(CourseFull);
                MainWindowViewModel.Instance.PageContent = new CoursePage();
            }
            else
            {
                CourseUpdate.Title = CourseFull.Title;
                CourseUpdate.Link = CourseFull.Link;
                CourseUpdate.Description = CourseFull.Description;
                await MainWindowViewModel.ApiClient.UpdateCourse(CourseUpdate);
                MainWindowViewModel.Instance.PageContent = new CoursePage();

            }
        }
    }
}