using System.Windows.Input;
using TypingApp.Commands;
using NavigationService = TypingApp.Services.NavigationService;

namespace TypingApp.ViewModels;

public class CreateExerciseViewModel : ViewModelBase
{
    private string _exerciseText;
    private string _exerciseName;

    public ICommand SaveButton { set; get; }
    public ICommand CancelButton { set; get; }
    
    public string ExerciseText
    {
        get => _exerciseText;
        set
        {
            _exerciseText = value;
            OnPropertyChanged();
        } 
    }

    public string ExerciseName
    {
        get => _exerciseName;
        set
        {
            _exerciseName = value;
            OnPropertyChanged();
        }
    }

    public CreateExerciseViewModel(NavigationService teacherDashboardNavigationService)
    {
        SaveButton = new CreateExerciseCommand(this);
        CancelButton = new CancelCommand(teacherDashboardNavigationService);
    }
}