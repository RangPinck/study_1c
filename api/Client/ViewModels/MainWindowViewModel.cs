using Avalonia.Controls;
using ReactiveUI;
using System.Collections.Generic;
using Tmds.DBus.Protocol;

namespace Client.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {


        private UserControl _pageContent = new CoursePage();

        public static Connection ApiClient = new Connection("http://localhost:7053/api/");


        public static MainWindowViewModel Instance;

            public MainWindowViewModel()
            {
                Instance = this;
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
    }
}
