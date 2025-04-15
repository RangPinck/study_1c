using Avalonia.Interactivity;
using Client.Models.Blocks;
using Client.Models.Courses;
using Newtonsoft.Json;
using ReactiveUI;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        private bool myPopup = false;

        private List<ShortBlockDTO> _blocks = new();

        public List<ShortBlockDTO> Blocks { get => _blocks; set => this.RaiseAndSetIfChanged(ref _blocks, value); }
        public bool MyPopup { get => myPopup; set => this.RaiseAndSetIfChanged(ref myPopup, value); }

        async Task GetData(ShortCourseDTO currentCourse)
        {
            var response = await MainWindowViewModel.ApiClient.GetBlock(currentCourse);
            Blocks = JsonConvert.DeserializeObject<List<ShortBlockDTO>>(response);
        }

        private void OnButtonClick(object? sender, RoutedEventArgs e)
        {
            MyPopup = true;
        }

        private void OnClosePopupClick(object? sender, RoutedEventArgs e)
        {
            MyPopup = false;
        }

    }
}