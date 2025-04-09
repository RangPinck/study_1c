using System.Net;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using System.Collections.Generic;
using Client.Models;
using Client.Models;
using Tmds.DBus.Protocol;

namespace Client.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private UserControl _pageContent = new CoursePage();

        public static ConnectionApi ApiClient = new ConnectionApi("http://localhost:5022/api/");


        public static MainWindowViewModel Instance;

            public MainWindowViewModel()
            {
                Instance = this;

                CheckConnection();
            }
            public UserControl PageContent { get => _pageContent; set => this.RaiseAndSetIfChanged(ref _pageContent, value); }
        public bool IsPaneOpen { get => _isPaneOpen; set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value); }

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
            var Check = await ApiClient.CheckAvailability() == HttpStatusCode.OK;
            }
        public UserControl PageContent { get => _pageContent; set => this.RaiseAndSetIfChanged(ref _pageContent, value); }
    }
}
