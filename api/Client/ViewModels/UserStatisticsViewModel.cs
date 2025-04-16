using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Models;
using Client.Models.Users;
using Newtonsoft.Json;
using ReactiveUI;

namespace Client.ViewModels
{
	public class UserStatisticsViewModel : ViewModelBase
	{
     
        
		private List<UserDTO> _users = new();

        public List<UserDTO> Users { get => _users; set => this.RaiseAndSetIfChanged(ref _users, value); }


        public UserStatisticsViewModel()
        {
           
            GetUsers();

        }

        async Task GetUsers()
        {
            var response = await MainWindowViewModel.ApiClient.GetAllUsers(MainWindowViewModel.Instance.CurrentUser.Token);

            Users = JsonConvert.DeserializeObject<List<UserDTO>>(response);
        }
    }
}