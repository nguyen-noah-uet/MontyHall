using System.Collections.ObjectModel;
using Windows.UI.Popups;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MontyHall;

public partial class GamePlayViewModel: ObservableObject
{
    private const int DOOR_NUM = 3;
    private int carIndex = Random.Shared.Next(0, DOOR_NUM);
    private int SelectedIndex { get; set; }

    [ObservableProperty] private string _doorImage0 = "door.png";
    [ObservableProperty] private string _doorImage1 = "door.png";
    [ObservableProperty] private string _doorImage2 = "door.png";
    [ObservableProperty] private Color _backgroundImage0 = Colors.Transparent;
    [ObservableProperty] private Color _backgroundImage1 = Colors.Transparent;
    [ObservableProperty] private Color _backgroundImage2 = Colors.Transparent;
    [ObservableProperty] private string[] _doorImage = Enumerable.Repeat("door.png", 3).ToArray();

    public ObservableCollection<Door> Doors { get; set; } = new();

    public GamePlayViewModel()
    {
        for (int i = 0; i < DOOR_NUM; i++)
        {
            Doors.Add(new Door());
        }
        Doors[carIndex].HasCar = true;
    }
    [RelayCommand]
    public async Task OpenDoor(int index)
    {
        await Shell.Current.DisplayAlert("Alert", "Now we open 1 of the 2 doors you didn't select.", "OK");
        var arr = new int[3] { 0, 1, 2 }.ToList();
        if (index == carIndex)
        {
            arr.Remove(carIndex);
            int rand = arr[Random.Shared.Next(0, 2)];
            OpenGoatDoor(rand);
        }
        else
        {
            arr.Remove(carIndex);
            arr.Remove(index);
            int rand = arr[0];
            OpenGoatDoor(rand);
        }

        var changed = await Shell.Current.DisplayAlert("Alert", "Do you want to change your chosen door?", "Yes", "No");
        if (changed)
        {
            
        }
        else
        {

        }
    }

    private void OpenGoatDoor(int i)
    {
        switch (i)
        {
            case 0:
                DoorImage0 = "goat.png";
                break;
            case 1:
                DoorImage1 = "goat.png";
                break;
            case 2:
                DoorImage2 = "goat.png";
                break;
        }
    }

    private void SelectDoor(int i)
    {
        BackgroundImage0 = Colors.Transparent;
        BackgroundImage1 = Colors.Transparent;
        BackgroundImage2 = Colors.Transparent;
        switch (i)
        {
            case 0:
                BackgroundImage0 = Colors.Green;
                break;
            case 1:
                BackgroundImage1 = Colors.Green;
                break;
            case 2:
                BackgroundImage2 = Colors.Green;
                break;
        }
    }
}