using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Models.Blocks;
using Client.Models.Courses;
using Newtonsoft.Json;
using ReactiveUI;

namespace Client.ViewModels
{
	public class BlockPageViewModel : ViewModelBase
	{


        public BlockPageViewModel()
        {
        }
        public BlockPageViewModel(ShortCourseDTO currentCourse)
        {
            GetData(currentCourse);
        }

        private List<ShortBlockDTO> _blocks = new();

        public List<ShortBlockDTO> Blocks { get => _blocks; set => this.RaiseAndSetIfChanged(ref _blocks, value); }

        async Task GetData(ShortCourseDTO currentCourse)
        {
            var response = await MainWindowViewModel.ApiClient.GetBlock(currentCourse);
            Blocks = JsonConvert.DeserializeObject<List<ShortBlockDTO>>(response);
        }


    }
}