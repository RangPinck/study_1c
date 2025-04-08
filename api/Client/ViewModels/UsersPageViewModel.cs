using System;
using System.Collections.Generic;
using Client.Models;
using ReactiveUI;

namespace Client.ViewModels
{
	public class UsersPageViewModel : ViewModelBase
	{

		private List<TestClass> _test = new();

        public List<TestClass> Test { get => _test; set => this.RaiseAndSetIfChanged(ref _test, value); }

        public UsersPageViewModel()
        {
            Test = new List<TestClass>();
            Test.Add(new TestClass() { BlockId = "1", BlockName = "Test1" });
            Test.Add(new TestClass() { BlockId = "2", BlockName = "Test2" });
            Test.Add(new TestClass() { BlockId = "3", BlockName = "Test3" });
            Test.Add(new TestClass() { BlockId = "4", BlockName = "Test4" });

        }

        public void ToAddUser()
        {
            MainWindowViewModel.Instance.PageContent = new AddEditUser();
        }
    }
}