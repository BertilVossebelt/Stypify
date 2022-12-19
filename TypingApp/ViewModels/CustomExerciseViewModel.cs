using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using NavigationService = TypingApp.Services.NavigationService; 
namespace TypingApp.ViewModels;

public class CustomExerciseViewModel : ViewModelBase
{
    private Lesson _lesson;
    
    public ICommand CheckCommand { get; set; }
    public ICommand BackButton { get; set; }
    
    public Lesson Lesson
    {
        get => _lesson;
        set
        {
            _lesson = value;
            OnPropertyChanged();
        }
    }

    public CustomExerciseViewModel(NavigationService studentDashboardViewModel)
    {
        BackButton = new NavigateCommand(studentDashboardViewModel);
        CheckCommand = new CheckCommand();   
    }
}