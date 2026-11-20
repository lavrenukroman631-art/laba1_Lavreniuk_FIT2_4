
using System.Text.Json;
using laba1_Lavreniuk_FIT2_4.Models;
using laba1_Lavreniuk_FIT2_4.Services;
using Microsoft.Maui.Networking;
using SQLite;

namespace laba1_Lavreniuk_FIT2_4.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly SQLiteAsyncConnection _database;
    private readonly Task _initialization;
    private readonly EncryptionService _encryptionService;

    public StudentRepository(EncryptionService encryptionService)
    {
        _encryptionService = encryptionService;

        string dbPath = Path.Combine(
            FileSystem.AppDataDirectory,
            "students.db3");

        _database = new SQLiteAsyncConnection(dbPath);
        _initialization = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await _database.CreateTableAsync<Student>();
        await _database.CreateTableAsync<SyncQueueItem>();
    }

    public async Task<List<Student>> GetAllAsync()
    {
        await _initialization;

        var students = await _database.Table<Student>().ToListAsync();

        foreach (var student in students)
        {
            student.FullName = await _encryptionService.DecryptAsync(
                student.FullNameEncrypted);
        }

        return students;
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        await _initialization;

        var student = await _database.FindAsync<Student>(id);

        if (student != null)
        {
            student.FullName = await _encryptionService.DecryptAsync(
                student.FullNameEncrypted);
        }

        return student;
    }

    public async Task<int> SaveAsync(Student student)
    {
        await _initialization;

        bool isNew = student.Id == 0;

        student.FullNameEncrypted =
            await _encryptionService.EncryptAsync(student.FullName);

        int result;

        if (isNew)
            result = await _database.InsertAsync(student);
        else
            result = await _database.UpdateAsync(student);

        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            await AddToQueueAsync(
                isNew ? "Insert" : "Update",
                student);
        }

        return result;
    }

    public async Task<int> DeleteAsync(Student student)
    {
        await _initialization;

        int result = await _database.DeleteAsync(student);

        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            await AddToQueueAsync("Delete", student);
        }

        return result;
    }

    private async Task AddToQueueAsync(string operation, Student student)
    {
        var queueItem = new SyncQueueItem
        {
            EntityType = "Student",
            Operation = operation,
            PayloadJson = JsonSerializer.Serialize(student),
            CreatedAt = DateTime.Now
        };

        await _database.InsertAsync(queueItem);
    }

    public async Task<List<SyncQueueItem>> GetPendingChangesAsync()
    {
        await _initialization;

        return await _database.Table<SyncQueueItem>()
            .OrderBy(item => item.CreatedAt)
            .ToListAsync();
    }

    public async Task RemoveFromQueueAsync(SyncQueueItem item)
    {
        await _initialization;
        await _database.DeleteAsync(item);
    }
}
