using Avalonia.Controls;
using ReactiveUI;
using Tmds.DBus.Protocol;

namespace Client.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private UserControl _pageContent = new UsersPage();

        public static Connection ApiClient = new Connection("http://localhost:7053/api/");


        public static MainWindowViewModel Instance;

            public MainWindowViewModel()
            {
                Instance = this;
            }

            public UserControl PageContent { get => _pageContent; set => this.RaiseAndSetIfChanged(ref _pageContent, value); }
      
    }
}
