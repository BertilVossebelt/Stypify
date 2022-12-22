using System.Collections.Generic;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Stores;
using NavigationService = TypingApp.Services.NavigationService;

namespace TypingApp.ViewModels;

public class LessonViewModel : ViewModelBase
{
    private readonly LessonStore _lessonStore;

    private Lesson _lesson;
    private Exercise _exercise;
    private string _userInputText;
    private List<Character> _auditedTextAsCharList;
    private bool _audited;

    public ICommand AuditButton { get; set; }
    public ICommand BackButton { get; set; }
    public ICommand NextExerciseButton { get; set; }

    public Lesson Lesson
    {
        get => _lesson;
        set
        {
            _lesson = value;
            OnPropertyChanged();
        }
    }

    public Exercise Exercise
    {
        get => _exercise;
        set
        {
            _exercise = value;
            OnPropertyChanged();
        }
    }

    public string UserInputText
    {
        get => _userInputText;
        set
        {
            _userInputText = value;
            OnPropertyChanged();
        }
    }

    public List<Character> AuditedTextAsCharList
    {
        get => _auditedTextAsCharList;
        set
        {
            _auditedTextAsCharList = value;
            OnPropertyChanged();
        }
    }

    public bool Audited
    {
        get => _audited;
        set
        {
            _audited = value;
            OnPropertyChanged();
        }
    }

    public LessonViewModel(NavigationService studentDashboardViewModel, LessonStore lessonStore, UserStore userStore)
    {
        _lessonStore = lessonStore;
        Lesson = lessonStore.CurrentLesson;
        Exercise = Lesson.Exercises[lessonStore.CurrentExercise];

        BackButton = new NavigateCommand(studentDashboardViewModel);
        AuditButton = new AuditExerciseCommand(this, lessonStore, userStore);
        NextExerciseButton = new NextExerciseCommand(lessonStore);

        lessonStore.AuditedExerciseCreated += AuditedExerciseCreatedHandler;
        lessonStore.NextExercise += NextExerciseHandler;
    }

    private void AuditedExerciseCreatedHandler(List<Character> characters)
    {
        AuditedTextAsCharList = characters;
        Audited = true;
    }

    private void NextExerciseHandler(int currentExercise)
    {
        Exercise = Lesson.Exercises[currentExercise];
        UserInputText = "";
        Audited = false;
    }
}