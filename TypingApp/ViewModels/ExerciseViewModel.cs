using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Stores;

namespace TypingApp.ViewModels;

public class ExerciseViewModel : ViewModelBase
{
    private ObservableCollection<Character> _textAsCharList;
    public ObservableCollection<Character> TextAsCharList
    {
        get => _textAsCharList;
        set
        {
            _textAsCharList = value;
            OnPropertyChanged();
        }
    }
    public ICommand BackButton { get; }

    public ExerciseViewModel(NavigationService studentDashboardNavigationService, User user, ExerciseStore exerciseStore)
    {
        BackButton = new NavigateCommand(studentDashboardNavigationService);
        exerciseStore.ExerciseCreated += OnExerciseChanged;
        exerciseStore.ExerciseUpdated += OnExerciseChanged;

        var generateExerciseCommand = new GenerateExerciseCommand(exerciseStore, user.Characters);
        generateExerciseCommand.Execute(this);
    }
    
    private void OnExerciseChanged(List<Character> characters)
    {
        TextAsCharList = new ObservableCollection<Character>(characters);
    }
}