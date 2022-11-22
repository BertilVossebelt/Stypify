using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Stores;

namespace TypingApp.ViewModels;

public class ExerciseViewModel : ViewModelBase
{
    private string _text { get; set; }
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged();
        }
    }
    public ICommand BackButton { get; }

    public ExerciseViewModel(string text, NavigationStore navigationStore, User user)
    {
        Text = text;
        BackButton = new BackCommand(navigationStore, user);
    }
}