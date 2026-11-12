
namespace laba1_Lavreniuk_FIT2_4.Services;

public interface IAppStateService
{
    string CurrentState { get; set; }
    DateTime? LastResumedAt { get; set; }
}
