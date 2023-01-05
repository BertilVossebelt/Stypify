using System.Collections.ObjectModel;
using NavigationService = TypingApp.Services.NavigationService;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;
using TypingApp.Commands;
using System.Windows.Input;
using TypingApp.Views;

namespace TypingApp.ViewModels;

public class CreateLessonViewModel : ViewModelBase
{
    public static CreateLessonViewModel createLessonViewModel;
    private ObservableCollection<Exercise>? _exercises;
    private readonly UserStore _userStore;
    private ObservableCollection<Exercise>? _selectedExercises;
    private ObservableCollection<Group>? _groups;
    private int _amountOfExercises;
    private string _name = null!;

    public ICommand SaveLessonCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public ICommand BackButton { get; set; }
    
    public UserStore UserStore { get; set; }
    
    public ObservableCollection<Exercise>? Exercises
    {
        get => _exercises;
        set
        {
            if (value == null) return;
            _exercises = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Exercise>? SelectedExercises
    {
        get => _selectedExercises;
        set
        {
            _selectedExercises = value;
            OnPropertyChanged();
        }
    }

    public int AmountOfExercises
    {
        get => _amountOfExercises;
        set
        {
            _amountOfExercises = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Group>? Groups
    {
        get => _groups;
        set
        {
            _groups = value;
            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public CreateLessonViewModel(UserStore userStore, NavigationService myLessonsService)
    {
        _userStore = userStore;
        
        Groups = new ObservableCollection<Group>();
        Exercises = new ObservableCollection<Exercise>();
        
        createLessonViewModel = this;
        SaveLessonCommand = new SaveLessonCommand(myLessonsService);
        CancelCommand = new CancelCommand(myLessonsService);
        BackButton = new CancelCommand(myLessonsService);

        if (userStore.Teacher == null) return;
        var groups = new TeacherProvider().GetGroups(userStore.Teacher.Id);
        groups?.ForEach(g => Groups?.Add(new Group((int)g["id"], (string)g["name"], (string)g["code"])));
        
        var exercises = new ExerciseProvider().GetAll(userStore.Teacher.Id);
        exercises?.ForEach(e => Exercises?.Add(new Exercise((string)e["text"], (string)e["name"], (int)e["id"])));
        AmountOfExercises = exercises?.Count ?? 0;
    }
    
    public void SelectItems()
    {
        var toBeSelectedExercises = new ObservableCollection<Exercise>();
        var toBeSelectedGroups = new ObservableCollection<Group>();
        if (_userStore.LessonId == null) return;

        var name = new LessonProvider().GetById((int)_userStore.LessonId);
        Name = (string)name["name"];

        var exercises = new LessonProvider().GetExercises((int)_userStore.LessonId);
        exercises?.ForEach(e => toBeSelectedExercises?.Add(new Exercise((string)e["text"], (string)e["name"], (int)e["id"])));

        foreach (var exercise in toBeSelectedExercises)
        {
            if (CreateLessonView.ExerciseListBox?.Items == null) break;
            foreach (Exercise listboxItem in CreateLessonView.ExerciseListBox.Items)
            {
                if (exercise.Id == listboxItem.Id)
                {
                    CreateLessonView.ExerciseListBox.SelectedItems.Add(listboxItem);
                }
            }
        }

        var groups = new LessonProvider().GetLinkedGroups((int)_userStore.LessonId);
        groups?.ForEach(e => toBeSelectedGroups.Add(new Group((int)e["id"], (string)e["name"], (string)e["code"])));

        foreach (var group in toBeSelectedGroups)
        {
            if (CreateLessonView.GroupListbox?.Items == null) break;
            foreach (Group listboxItem in CreateLessonView.GroupListbox.Items)
            {
                if (group.GroupId == listboxItem.GroupId)
                {
                    CreateLessonView.GroupListbox.SelectedItems.Add(listboxItem);
                }
            }
        }
    }
}