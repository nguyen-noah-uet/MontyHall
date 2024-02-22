using System.Text;

namespace MontyHall;

public partial class GamePlay : ContentPage
{
    private int changeDecisionGames;
    private int changeDecisionWinGames;
    private int noChangeDecisionGames;
    private int noChangeDecisionWinGames;
    private bool hasChangeDecision;
    private List<int> list = new int[3] { 0, 1, 2 }.ToList();
    private int selectedIndex = -1;
    private int revealIndex = -1;
    //private int carIndex = Random.Shared.Next(0, 3);
    private int carIndex = 1;
	public GamePlay(GamePlayViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    private async void Reset()
    {
        selectedIndex = -1;
        revealIndex = -1;
        carIndex = Random.Shared.Next(0, 3);
        BtnDoor0.Source = "door.png";
        BtnDoor1.Source = "door.png";
        BtnDoor2.Source = "door.png";
        BtnDoor0.IsEnabled = true;
        BtnDoor1.IsEnabled = true;
        BtnDoor2.IsEnabled = true;
        BtnDoor0.BorderColor = Colors.Transparent;
        BtnDoor1.BorderColor = Colors.Transparent;
        BtnDoor2.BorderColor = Colors.Transparent;
        hasChangeDecision = false;
        LbMessage.Text = "";
    }
    private async void BtnDoor0_OnClicked(object? sender, EventArgs e)
    {
        await PlayGame(0);
    }

    private async void BtnDoor1_OnClicked(object? sender, EventArgs e)
    {
        await PlayGame(1);
    }

    private async void BtnDoor2_OnClicked(object? sender, EventArgs e)
    {
        await PlayGame(2);
    }
    private async Task PlayGame(int i)
    {
        selectedIndex = i;
        SelectDoor(GetDoor(i));
        Log($"You select door {selectedIndex + 1}.");
        OpenGoatDoor();
        Log($"We open door {revealIndex + 1} that has a goat.");
        int remainIndex = list.Except(new List<int>() { selectedIndex, revealIndex }).ToList()[0];
        hasChangeDecision = await Shell.Current.DisplayAlert("Alert", $"You select door {selectedIndex + 1}\nWe open door {revealIndex + 1} that has a goat.\nDo you want to change your selection to choose door {remainIndex + 1}?", "Yes", "No");
        if (hasChangeDecision)
        {
            selectedIndex = remainIndex;
            Log($"You change your selection to door {selectedIndex + 1}.");
        }
        else
        {
            Log($"You make no change to your selection");
            GetDoor(remainIndex).IsEnabled = false;
        }

        ImageButton finalDoor = GetDoor(selectedIndex);
        SelectDoor(finalDoor);
        BtnShowResult.IsEnabled = true;
    }

    private ImageButton GetDoor(int i)
    {
        switch (i)
        {
            case 0:
                return BtnDoor0;
            case 1:
                return BtnDoor1;
            case 2:
                return BtnDoor2;
            default:
                return null;
        }
    }

    private void OpenGoatDoor()
    {
        if (selectedIndex == carIndex)
        {
            revealIndex = list.Except(new List<int>() { selectedIndex }).ToList()[0];
        }
        else
        {
            revealIndex = list.Except(new List<int>() { selectedIndex, carIndex }).ToList()[0];
        }
        var goatDoor = GetDoor(revealIndex);
        goatDoor.Source = "goat.png";
        goatDoor.IsEnabled = false;
    }

    private void SelectDoor(ImageButton btnDoor)
    {
        BtnDoor0.BorderColor = Colors.Transparent;
        BtnDoor1.BorderColor = Colors.Transparent;
        BtnDoor2.BorderColor = Colors.Transparent;
        btnDoor.IsEnabled = false;
        btnDoor.BorderColor = Colors.Yellow;
    }


    private void BtnReset_OnClicked(object? sender, EventArgs e)
    {
        Reset();
    }

    private void Log(string message)
    {
        LbMessage.Text += message + "\n";
    }

    private async void BtnShowResult_OnClicked(object? sender, EventArgs e)
    {
        foreach (var i in list)
        {
            if(i == carIndex)
                GetDoor(i).Source = "car.png";
            else
                GetDoor(i).Source = "goat.png";
        }

        if (selectedIndex == carIndex)
        {
            Log("You win a car!!");
            await Shell.Current.DisplayAlert("Yeah", "You won a car!!", "Oki");
            if (hasChangeDecision)
                changeDecisionWinGames += 1;
            else
                noChangeDecisionWinGames += 1;
        }
        else
        {
            Log("You have a goat.");
            await Shell.Current.DisplayAlert("Ahihi", "You have a goat.", ":)))");
        }

        if (hasChangeDecision)
            changeDecisionGames += 1;
        else
            noChangeDecisionGames += 1;
        BtnShowResult.IsEnabled = false;
    }

    private async void BtnStat_OnClicked(object? sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        if (noChangeDecisionGames > 0)
        {
            sb.AppendLine(
                $"No change decision games: {noChangeDecisionGames}");
            sb.AppendLine(
                $"No change decision win games: {noChangeDecisionWinGames} ({100.0 * noChangeDecisionWinGames / noChangeDecisionGames:##.##}%)");
            sb.AppendLine();
        }

        if (changeDecisionGames > 0)
        {
            sb.AppendLine(
                $"Change decision games: {changeDecisionGames}");
            sb.AppendLine(
                $"Change decision win games: {changeDecisionWinGames} ({100.0 * changeDecisionWinGames / changeDecisionGames:##.##}%)");
            sb.AppendLine();

        }
        if (noChangeDecisionGames + changeDecisionGames > 0)
        {
            sb.AppendLine(
                $"Total games: {changeDecisionGames + noChangeDecisionGames}");
            sb.AppendLine(
                $"Win games: {changeDecisionWinGames + noChangeDecisionWinGames} ({100.0 * (changeDecisionWinGames + noChangeDecisionWinGames) / (changeDecisionGames + noChangeDecisionGames):##.##}%)");
            sb.AppendLine();

        }
        await Shell.Current.DisplayAlert("Stat", sb.ToString(), "OK");
    }
}