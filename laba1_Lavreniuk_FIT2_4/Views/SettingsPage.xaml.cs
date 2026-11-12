
using Microsoft.Maui.Storage;

namespace laba1_Lavreniuk_FIT2_4.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

        string savedTheme = Preferences.Get("AppTheme", "Світла");

        ThemePicker.SelectedIndex =
            savedTheme == "Темна" ? 1 : 0;
    }

    private void OnThemeChanged(object sender, EventArgs e)
    {
        if (ThemePicker.SelectedIndex == -1)
            return;

        bool isDark = ThemePicker.SelectedIndex == 1;

        Application.Current!.UserAppTheme =
            isDark ? AppTheme.Dark : AppTheme.Light;

        Preferences.Set(
            "AppTheme",
            isDark ? "Темна" : "Світла");
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
