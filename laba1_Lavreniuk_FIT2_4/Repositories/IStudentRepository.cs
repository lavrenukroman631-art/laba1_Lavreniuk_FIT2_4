
using laba1_Lavreniuk_FIT2_4.Models;

namespace laba1_Lavreniuk_FIT2_4.Repositories;

public interface IStudentRepository
{
    Task<List<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task<int> SaveAsync(Student student);
    Task<int> DeleteAsync(Student student);

    Task<List<SyncQueueItem>> GetPendingChangesAsync();
    Task RemoveFromQueueAsync(SyncQueueItem item);
}
