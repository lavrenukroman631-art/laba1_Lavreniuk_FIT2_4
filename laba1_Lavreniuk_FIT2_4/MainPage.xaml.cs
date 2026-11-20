
using laba1_Lavreniuk_FIT2_4.ViewModels;
using laba1_Lavreniuk_FIT2_4.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using laba1_Lavreniuk_FIT2_4.Services;

namespace laba1_Lavreniuk_FIT2_4;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        var services = Application.Current!
            .Handler!
            .MauiContext!
            .Services;

        var repository = services.GetRequiredService<IStudentRepository>();

        BindingContext = new StudentViewModel(repository);

        try
        {
            var syncService = services.GetRequiredService<SyncService>();

            _ = syncService.SyncPendingChangesAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine("ОШИБКА SYNC SERVICE:");
            Debug.WriteLine(ex.ToString());
        }

        
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
