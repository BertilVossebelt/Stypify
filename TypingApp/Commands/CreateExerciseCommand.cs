using System.Windows;
using System.Windows.Input;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;
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

    public override void Execute(object? parameter)
    {
        string? message;
        // Check if name is provided.
        if (_createExerciseViewModel.ExerciseName is "" or null)
        {
            message = "Er is geen naam ingevoerd.";
            MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        
        // Check if exercise text is provided.
        if (_createExerciseViewModel.ExerciseText is "" or null)
        {
            message = "Er is geen tekst ingevoerd.";
            MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        
        // Store exercise if user is a teacher
        if (_userStore.Teacher != null)
        {
            new ExerciseProvider().Create(_userStore.Teacher.Id, _createExerciseViewModel.ExerciseName,
                _createExerciseViewModel.ExerciseText);
            
            message = $"{_createExerciseViewModel.ExerciseName} is opgeslagen.";
            MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        // Return back to previous page.
        var navigateCommand = new NavigateCommand(_teacherDashboardNavigationService);
        navigateCommand.Execute(this);
    }
}