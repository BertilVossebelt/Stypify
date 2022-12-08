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
        if (_userStore.Teacher != null)
        {
            new ExerciseProvider().Create(_userStore.Teacher.Id, _createExerciseViewModel.ExerciseName,
                _createExerciseViewModel.ExerciseText);
        }

        var message = $"{_createExerciseViewModel.ExerciseName} is opgeslagen.";
        MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        var navigateCommand = new NavigateCommand(_teacherDashboardNavigationService);
        navigateCommand.Execute(this);
    }
}