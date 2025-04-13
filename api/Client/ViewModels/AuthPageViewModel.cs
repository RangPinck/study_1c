using System;
using System.Collections.Generic;
using ReactiveUI;

namespace Client.ViewModels
{
	public class AuthPageViewModel : ViewModelBase
	{

		private string _email = "admin@admin.com";

		private string _password = "admin1cdbapi";

        public string Email { get => _email; set => this.RaiseAndSetIfChanged(ref _email , value); }
        public string Password { get => _password; set => this.RaiseAndSetIfChanged(ref _password, value); }

        public void Authorize()
		{
            MainWindowViewModel.Instance.Authorize(Email, Password);			
		}

	}
}