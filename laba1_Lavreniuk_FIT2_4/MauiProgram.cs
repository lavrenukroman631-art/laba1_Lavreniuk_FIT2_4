
using Microsoft.Extensions.Logging;
using laba1_Lavreniuk_FIT2_4.Services;
using laba1_Lavreniuk_FIT2_4.Repositories;

namespace laba1_Lavreniuk_FIT2_4
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IAppStateService, AppStateService>();

            // Реєстрація репозиторію студентів
            builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
            builder.Services.AddSingleton<SyncService>();
            builder.Services.AddSingleton<EncryptionService>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}