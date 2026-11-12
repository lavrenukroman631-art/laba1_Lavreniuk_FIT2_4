
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace laba1_Lavreniuk_FIT2_4
{
    [Activity(
        Theme = "@style/Maui.SplashTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize |
                               ConfigChanges.Orientation |
                               ConfigChanges.UiMode |
                               ConfigChanges.ScreenLayout |
                               ConfigChanges.SmallestScreenSize |
                               ConfigChanges.Density,
        LaunchMode = LaunchMode.SingleTop,
        Exported = true)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[]
        {
            Intent.CategoryDefault,
            Intent.CategoryBrowsable
        },
        DataScheme = "https",
        DataHost = "studentapp.example.com",
        DataPathPrefix = "/student")]

    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[]
    {
        Intent.CategoryDefault,
        Intent.CategoryBrowsable
    },
    DataScheme = "studentapp",
    DataHost = "student",
    DataPathPrefix = "/")]

    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HandleDeepLink(Intent);
        }

        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);

            if (intent != null)
            {
                Intent = intent;
                HandleDeepLink(intent);
            }
        }

        private void HandleDeepLink(Intent? intent)
        {
            if (intent?.Data == null)
                return;

            string? studentId = intent.Data.LastPathSegment;

            if (!string.IsNullOrWhiteSpace(studentId))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync(
                        $"studentdetail?StudentId={studentId}");
                });
            }
        }
    }
}
