using System.Net;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using Client.Models;
using Tmds.DBus.Protocol;

namespace Client.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private UserControl _pageContent = new UsersPage();

        public static ConnectionApi ApiClient = new ConnectionApi("http://localhost:5022/api/");


        public static MainWindowViewModel Instance;

            public MainWindowViewModel()
            {
                Instance = this;

                CheckConnection();
            }


            private async Task CheckConnection()
            {
            var Check = await ApiClient.CheckAvailability() == HttpStatusCode.OK;
            }
        public UserControl PageContent { get => _pageContent; set => this.RaiseAndSetIfChanged(ref _pageContent, value); }
      
    }
}
