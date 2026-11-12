
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using laba1_Lavreniuk_FIT2_4.Models;

namespace laba1_Lavreniuk_FIT2_4.ViewModels;

public class StudentDetailViewModel : INotifyPropertyChanged, IQueryAttributable
{
    private Student? _originalStudent;
    private string _fullName = string.Empty;
    private string _group = string.Empty;
    private double _averageScore;
    private string _editedAverageScore = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string FullName
    {
        get => _fullName;
        set
        {
            _fullName = value;
            OnPropertyChanged(nameof(FullName));
        }
    }

    public string Group
    {
        get => _group;
        set
        {
            _group = value;
            OnPropertyChanged(nameof(Group));
        }
    }

    public double AverageScore
    {
        get => _averageScore;
        set
        {
            _averageScore = value;
            OnPropertyChanged(nameof(AverageScore));
        }
    }

    public string EditedAverageScore
    {
        get => _editedAverageScore;
        set
        {
            _editedAverageScore = value;
            OnPropertyChanged(nameof(EditedAverageScore));
        }
    }

    public ICommand GoBackCommand { get; }
    public ICommand SaveCommand { get; }

    public StudentDetailViewModel()
    {
        GoBackCommand = new Command(async () =>
            await Shell.Current.GoToAsync(".."));

        SaveCommand = new Command(async () => await SaveAsync());
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("SelectedStudent", out var value)
            && value is Student student)
        {
            SetStudent(student);
            return;
        }

        if (query.TryGetValue("StudentId", out var idValue)
            && int.TryParse(idValue?.ToString(), out int studentId))
        {
            var viewModel = StudentViewModel.Instance;

            var foundStudent = viewModel?.Students
                .FirstOrDefault(s => s.Id == studentId);

            if (foundStudent != null)
                SetStudent(foundStudent);
        }
    }

    private void SetStudent(Student student)
    {
        _originalStudent = student;

        FullName = student.FullName;
        Group = student.Group;
        AverageScore = student.AverageScore;

        EditedAverageScore = student.AverageScore.ToString(
            CultureInfo.CurrentCulture);
    }

    private async Task SaveAsync()
    {
        if (_originalStudent == null)
            return;

        bool validScore = double.TryParse(
            EditedAverageScore,
            NumberStyles.Any,
            CultureInfo.CurrentCulture,
            out double newScore);

        if (!validScore)
        {
            validScore = double.TryParse(
                EditedAverageScore,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out newScore);
        }

        if (!validScore)
            return;

        Student updatedStudent = new Student
        {
            Id = _originalStudent.Id,
            FullName = FullName,
            Group = Group,
            AverageScore = newScore
        };

        var parameters = new Dictionary<string, object>
        {
            { "OriginalStudent", _originalStudent },
            { "UpdatedStudent", updatedStudent }
        };

        await Shell.Current.GoToAsync("..", parameters);
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
