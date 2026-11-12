
namespace laba1_Lavreniuk_FIT2_4.Services;

public class AppStateService : IAppStateService
{
    public string CurrentState { get; set; } = "Активний";

    public DateTime? LastResumedAt { get; set; }
}
