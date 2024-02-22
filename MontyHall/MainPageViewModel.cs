
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MontyHall
{
    public partial class MainPageViewModel : ObservableObject
    {
        [RelayCommand]
        public async Task GoToGamePlay()
        {
            await Shell.Current.GoToAsync("/GamePlay");
        }
    }
}
