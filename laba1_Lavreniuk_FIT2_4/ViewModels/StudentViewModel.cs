
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using laba1_Lavreniuk_FIT2_4.Models;
using laba1_Lavreniuk_FIT2_4.Repositories;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace laba1_Lavreniuk_FIT2_4.ViewModels;

public class StudentViewModel : INotifyPropertyChanged, IQueryAttributable
{
    public static StudentViewModel? Instance { get; private set; }

    private readonly IStudentRepository _repository;
    private Student _student = new();

    public ObservableCollection<Student> Students { get; } = new();

    public ICommand AddStudentCommand { get; }
    public ICommand OpenDetailsCommand { get; }
    public ICommand LoadCommand { get; }
    public ICommand DeleteStudentCommand { get; }

    public string FullName
    {
        get => _student.FullName;
        set
        {
            if (_student.FullName != value)
            {
                _student.FullName = value;
                Preferences.Set("FormFullName", value ?? string.Empty);
                OnPropertyChanged();
                ((Command)AddStudentCommand).ChangeCanExecute();
            }
        }
    }

    public string Group
    {
        get => _student.Group;
        set
        {
            if (_student.Group != value)
            {
                _student.Group = value;
                Preferences.Set("FormGroup", value ?? string.Empty);
                OnPropertyChanged();
            }
        }
    }

    public double AverageScore
    {
        get => _student.AverageScore;
        set
        {
            if (_student.AverageScore != value)
            {
                _student.AverageScore = value;
                Preferences.Set("FormAverageScore", value);
                OnPropertyChanged();
            }
        }
    }

    public StudentViewModel(IStudentRepository repository)
    {
        Instance = this;
        _repository = repository;

        _student.FullName = Preferences.Get("FormFullName", string.Empty);
        _student.Group = Preferences.Get("FormGroup", string.Empty);
        _student.AverageScore = Preferences.Get("FormAverageScore", 0.0);

        AddStudentCommand = new Command(
            async () => await AddStudentAsync(),
            CanAddStudent);

        OpenDetailsCommand = new Command<Student>(OpenDetails);

        LoadCommand = new Command(
            async () => await LoadAsync());
        DeleteStudentCommand = new Command<Student>(
            async (student) => await DeleteStudentAsync(student));
    }

    private async Task AddStudentAsync()
    {
        Student newStudent = new()
        {
            FullName = FullName,
            Group = Group,
            AverageScore = AverageScore
        };

        await _repository.SaveAsync(newStudent);
        Students.Add(newStudent);

        FullName = string.Empty;
        Group = string.Empty;
        AverageScore = 0;
    }

    private bool CanAddStudent()
    {
        return !string.IsNullOrWhiteSpace(FullName);
    }

    private async void OpenDetails(Student? student)
    {
        if (student == null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "SelectedStudent", student }
        };

        await Shell.Current.GoToAsync("studentdetail", parameters);
    }

    private async Task LoadAsync()
    {
        try
        {
            var students = await _repository.GetAllAsync();

            Students.Clear();

            foreach (var student in students)
                Students.Add(student);

            Debug.WriteLine("Список студентів завантажено з SQLite.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Помилка завантаження: {ex.Message}");
        }
    }

    private async Task DeleteStudentAsync(Student? student)
    {
        if (student == null)
            return;

        bool confirm = await Shell.Current.DisplayAlert(
            "Видалення студента",
            $"Ви дійсно хочете видалити {student.FullName}?",
            "Так",
            "Ні");

        if (!confirm)
            return;

        await _repository.DeleteAsync(student);
        Students.Remove(student);
    }
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("OriginalStudent", out var originalValue)
            && originalValue is Student originalStudent
            && query.TryGetValue("UpdatedStudent", out var updatedValue)
            && updatedValue is Student updatedStudent)
        {
            await _repository.SaveAsync(updatedStudent);

            int index = Students.IndexOf(originalStudent);

            if (index >= 0)
                Students[index] = updatedStudent;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
