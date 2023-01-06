using System.Windows;
using TypingApp.Services;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Commands;

public class CreateExerciseCommand : CommandBase
{
    private readonly CreateExerciseViewModel _createExerciseViewModel;
    private readonly UserStore _userStore;
    private readonly NavigationService _teacherDashboardNavigationService;

    public CreateExerciseCommand(CreateExerciseViewModel createExerciseViewModel,
        NavigationService teacherDashboardNavigationService, UserStore userStore)
    {
        _teacherDashboardNavigationService = teacherDashboardNavigationService;
        _createExerciseViewModel = createExerciseViewModel;
        _userStore = userStore;
    }

    /*
     * Creates a new exercise and adds it to the database.
     * ---------------------------------------------------
     * Method should only be used for teachers.
     */
    public override void Execute(object? parameter)
    {
        string? message;

        // Validate input.
        if (ValidateExerciseData())
        {
            message = "Er is geen naam of geen tekst ingevoerd."; 
            MessageBox.Show(message, "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        
        // Store exercise if user is a teacher.
        if (_userStore.Teacher != null)
        {
            message = $"{_createExerciseViewModel.ExerciseName} is opgeslagen.";
            MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        // Return back to previous page.
        var navigateCommand = new NavigateCommand(_teacherDashboardNavigationService);
        navigateCommand.Execute(this);
    }

    /*
     * Validates the input from the user.
     */
    private bool ValidateExerciseData()
    {
        if (_createExerciseViewModel.ExerciseName is "" or null) return false;
        return _createExerciseViewModel.ExerciseText is not ("" or null);
    }
}