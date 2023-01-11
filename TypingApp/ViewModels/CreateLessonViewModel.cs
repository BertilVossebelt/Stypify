using System.Collections.ObjectModel;
using NavigationService = TypingApp.Services.NavigationService;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;
using TypingApp.Commands;
using System.Windows.Input;
using TypingApp.Views;
using System;

namespace TypingApp.ViewModels;

public class CreateLessonViewModel : ViewModelBase
{
    public static CreateLessonViewModel createLessonViewModel;
    private ObservableCollection<Exercise>? _exercises;
    private ObservableCollection<Exercise>? _selectedExercises;
    private ObservableCollection<Group>? _groups;
    private int _amountOfExercises;
    private string _name = null!;
    

    public ICommand SaveLessonCommand { get; set; }
    public ICommand CancelCommand { get; set; }
    public ICommand BackButton { get; set; }
    
    public UserStore UserStore { get; set; }
    public LessonStore LessonStore { get; private set; }
    
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

    public CreateLessonViewModel(UserStore userStore, LessonStore lessonStore, NavigationService myLessonsService)
    {
        //populate variables
        UserStore = userStore;
        LessonStore = lessonStore;
        
        Groups = new ObservableCollection<Group>();
        Exercises = new ObservableCollection<Exercise>();
        
        createLessonViewModel = this;
        SaveLessonCommand = new SaveLessonCommand(myLessonsService);
        CancelCommand = new CancelCommand(myLessonsService);
        BackButton = new CancelCommand(myLessonsService);

        if (userStore.Teacher == null) return;
        //get all groups of teacher from database and populate groups
        var groups = new TeacherProvider().GetGroups(userStore.Teacher.Id);
        groups?.ForEach(g => Groups?.Add(new Group((int)g["id"], (string)g["name"], (string)g["code"])));

        //get all exercises of teacher from database and populate exercises
        var exercises = new ExerciseProvider().GetAll(userStore.Teacher.Id);
        exercises?.ForEach(e => Exercises?.Add(new Exercise((string)e["text"], (string)e["name"], (int)e["id"])));
        AmountOfExercises = exercises?.Count ?? 0;
    }
    
    //Preselect items when view is loaded
    public void SelectItems()
    {
        var toBeSelectedExercises = new ObservableCollection<Exercise>();
        var toBeSelectedGroups = new ObservableCollection<Group>();

        if (LessonStore.CurrentLesson == null) return;

        //get name of selectedlesson and populates it
        var name = new LessonProvider().GetById(LessonStore.CurrentLesson.Id);
        Name = (string)name["name"];

        //get exercises of lesson and populates it
        var exercises = new LessonProvider().GetExercises((int)LessonStore.CurrentLesson.Id);
        exercises?.ForEach(e => toBeSelectedExercises?.Add(new Exercise((string)e["text"], (string)e["name"], (int)e["id"])));

        foreach (var exercise in toBeSelectedExercises)
        {
            //check if listbox is empty
            if (CreateLessonView.ExerciseListBox?.Items == null) break;
            //goes trough all exercises of teacher and preselects matching items from already existing lesson
            foreach (Exercise listboxItem in CreateLessonView.ExerciseListBox.Items)
            {
                if (exercise.Id == listboxItem.Id)
                {
                    CreateLessonView.ExerciseListBox.SelectedItems.Add(listboxItem);
                }
            }
        }
        //get groups of lesson and populates it
        var groups = new LessonProvider().GetLinkedGroups((int)LessonStore.CurrentLesson.Id);
        groups?.ForEach(e => toBeSelectedGroups.Add(new Group((int)e["id"], (string)e["name"], (string)e["code"])));

        foreach (var group in toBeSelectedGroups)
        {   
            //check if listbox is empty
            if (CreateLessonView.GroupListbox?.Items == null) break;
            //goes trough all groups of teacher and preselects matching items from already existing lesson
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