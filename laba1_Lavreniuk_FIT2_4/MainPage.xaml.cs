
using laba1_Lavreniuk_FIT2_4.ViewModels;
using System.Diagnostics;

namespace laba1_Lavreniuk_FIT2_4;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new StudentViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var viewModel = (StudentViewModel)BindingContext;
        viewModel.LoadCommand.Execute(null);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        Debug.WriteLine("Головна сторінка прихована.");
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("settings");
    }
}
