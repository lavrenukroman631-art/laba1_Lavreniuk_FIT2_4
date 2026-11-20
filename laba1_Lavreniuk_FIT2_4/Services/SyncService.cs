
using System.Diagnostics;
using laba1_Lavreniuk_FIT2_4.Repositories;
using Microsoft.Maui.Networking;

namespace laba1_Lavreniuk_FIT2_4.Services;

public class SyncService
{
    private readonly IStudentRepository _repository;
    private readonly SemaphoreSlim _syncLock = new(1, 1);

    public SyncService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task SyncPendingChangesAsync()
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            Debug.WriteLine("Немає інтернету. Очікуємо підключення.");
            return;
        }

        if (!await _syncLock.WaitAsync(0))
            return;

        try
        {
            var changes = await _repository.GetPendingChangesAsync();

            foreach (var item in changes)
            {
                Debug.WriteLine(
                    $"Синхронізація: {item.Operation}, {item.EntityType}");

                // Імітація відправлення на тестовий сервер
                await Task.Delay(500);

                await _repository.RemoveFromQueueAsync(item);

                Debug.WriteLine($"Зміна {item.Id} синхронізована.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Помилка синхронізації: {ex}");
        }
        finally
        {
            _syncLock.Release();
        }
    }
}
