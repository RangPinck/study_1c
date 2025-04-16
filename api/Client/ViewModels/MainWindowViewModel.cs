using System.Net;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using System.Collections.Generic;
using Client.Models;
using Client.Models.Account;
using Avalonia.Controls.Chrome;
using MsBox.Avalonia;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Client.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        UserResponse currentUser = new UserResponse();

        private bool _paneVisibility = false;


        private UserControl _pageContent = new AuthPage();

        public static ConnectionApi ApiClient = new ConnectionApi("http://localhost:5022/api/");


        public static MainWindowViewModel Instance;

            public MainWindowViewModel()
            {
                Instance = this;

            CheckConnection();
            }
        public UserControl PageContent { get => _pageContent; set => this.RaiseAndSetIfChanged(ref _pageContent, value); }
        public bool IsPaneOpen { get => _isPaneOpen; set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value); }
        public UserResponse CurrentUser { get => currentUser; set => this.RaiseAndSetIfChanged(ref currentUser, value); }
        public bool PaneVisibility { get => _paneVisibility; set => this.RaiseAndSetIfChanged(ref _paneVisibility, value); }
        public bool IsOnline { get => _isOnline; set => this.RaiseAndSetIfChanged(ref _isOnline, value); }

        private bool _isOnline = false;



        private bool _isPaneOpen = false;
        public void PaneState()
        {
          IsPaneOpen = !IsPaneOpen;
        }

        public void ToUsers()
        {
            Instance.PageContent = new UsersPage();
        }

        public void ToStatistics()
        {
            Instance.PageContent = new UserStatistics();
        }
        public void ToCourses()
        {
            Instance.PageContent = new CoursePage();
        }
            private async Task CheckConnection()
            {
             IsOnline = await ApiClient.CheckAvailability() == HttpStatusCode.OK;

            }
        public class ApiErrorResponse
        {
            [JsonPropertyName("title")]
            public string Title { get; set; }

            [JsonPropertyName("errors")]
            public Dictionary<string, List<string>> Errors { get; set; }
        }

        

        public async Task Authorize(string Email, string Password)
        {
            LogInDTO logIn = new LogInDTO()
            {
                Email = Email,
                Password = Password
            };
            var response = await ApiClient.LogInUser(logIn);

            CurrentUser = JsonConvert.DeserializeObject<UserResponse>(response);

            if (IsOnline)
            {

                if (CurrentUser != null)
                {
                    if (CurrentUser.IsFirst)
                        PageContent = new HelloPage();
                    else
                        PageContent = new CoursePage();

                }
            }
            else
            {
                ErrorMessage("Нет ответа от АПИ!","Нет соединения с АПИ! Попробуйте ещё раз позже");
            }
        }

        public async Task ErrorMessage(string title, string message)
        {
            await MessageBoxManager.GetMessageBoxStandard(title, message, MsBox.Avalonia.Enums.ButtonEnum.Ok).ShowAsync();
        }
    }
}
