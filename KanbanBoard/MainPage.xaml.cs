using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

namespace KanbanBoard;

public partial class MainPage : ContentPage
{
    private readonly IPath _path;

    public MainPage(MainPageViewModel viewModel, IPath path)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _path = path;
    }

    private async void ResetButton_OnClicked(object sender, EventArgs e)
    {
        var options = new SnackbarOptions
        {
            BackgroundColor = Colors.Red,
            TextColor = Colors.Green,
            ActionButtonTextColor = Colors.Yellow,
            CornerRadius = new CornerRadius(10),
            Font = Font.SystemFontOfSize(14),
        };
        await ResetButton.DisplaySnackbar(
            "All your data will be deleted. Application will be closed",
            DeleteDbAndCloseApp,
            "Confirm and delete",
            TimeSpan.FromSeconds(5),
            options);
    }

    private void DeleteDbAndCloseApp()
    {
        var dbPath = _path.GetDatabasePath();
        _path.DeleteFile(dbPath);
        Environment.Exit(0);
    }
}
