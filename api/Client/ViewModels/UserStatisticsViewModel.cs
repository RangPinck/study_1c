using System;
using System.Collections.Generic;
using Client.Models;
using ReactiveUI;

namespace Client.ViewModels
{
	public class UserStatisticsViewModel : ViewModelBase
	{
     

            private List<TestClass> _test = new();

            public List<TestClass> Test { get => _test; set => this.RaiseAndSetIfChanged(ref _test, value); }

        private List<TestClass> _test2 = new();

        public List<TestClass> Test2 { get => _test2; set => this.RaiseAndSetIfChanged(ref _test2, value); }

        public UserStatisticsViewModel()
            {
                Test = new List<TestClass>();
                Test.Add(new TestClass() { BlockId = "1", BlockName = "Test1" });
                Test.Add(new TestClass() { BlockId = "2", BlockName = "Test2" });
                Test.Add(new TestClass() { BlockId = "3", BlockName = "Test3" });
                Test.Add(new TestClass() { BlockId = "4", BlockName = "Test4" });


            Test2 = new List<TestClass>();
            Test2.Add(new TestClass() { BlockId = "1", BlockName = "Test1" });
            Test2.Add(new TestClass() { BlockId = "2", BlockName = "Test2" });
            Test2.Add(new TestClass() { BlockId = "3", BlockName = "Test3" });
            Test2.Add(new TestClass() { BlockId = "4", BlockName = "Test4" });
        }
    }
}