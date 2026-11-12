
using System.Diagnostics;
using laba1_Lavreniuk_FIT2_4.Services;

namespace laba1_Lavreniuk_FIT2_4
{
    public partial class App : Application
    {
        private readonly IAppStateService _appStateService;

        public App(IAppStateService appStateService)
        {
            InitializeComponent();

            string savedTheme = Preferences.Get("AppTheme", "Світла");

            UserAppTheme = savedTheme == "Темна"
                ? AppTheme.Dark
                : AppTheme.Light;

            _appStateService = appStateService;

            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);

            window.Stopped += (sender, args) =>
            {
                _appStateService.CurrentState = "Фоновий режим";
                Debug.WriteLine("Стан сесії: " + _appStateService.CurrentState);
            };

            window.Resumed += (sender, args) =>
            {
                _appStateService.CurrentState = "Активний";
                _appStateService.LastResumedAt = DateTime.Now;

                Debug.WriteLine("Стан сесії: " + _appStateService.CurrentState);
                Debug.WriteLine("Останнє відновлення: " + _appStateService.LastResumedAt);
            };

            window.Destroying += (sender, args) =>
            {
                _appStateService.CurrentState = "Завершення";
                Debug.WriteLine("Стан сесії: " + _appStateService.CurrentState);
            };

            return window;
        }
    }
}
