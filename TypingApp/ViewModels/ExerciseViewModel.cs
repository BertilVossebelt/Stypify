using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Windows.Documents;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Stores;

namespace TypingApp.ViewModels;

public class ExerciseViewModel : ViewModelBase
{
    private List<Character> _textAsCharList;
    private string _text;
    private int _currentIndex;

    public ICommand BackButton { get; }
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged();
        }
    }
    public List<Character> TextAsCharList
    {
        get => _textAsCharList;
        set
        {
            _textAsCharList = value;
            Console.WriteLine("Value in ViewModel changed");
            OnPropertyChanged();
        }
    }
    
    public ExerciseViewModel(string text, NavigationStore navigationStore, User user)
    {
        BackButton = new BackCommand(navigationStore, user);
        
        var textAsCharList = text.Select(c => new Character(c)).ToList();
        TextAsCharList = textAsCharList;
    }
    
    public void HandleTextInput(object sender, TextCompositionEventArgs e)
    {
        var keyChar = (char)System.Text.Encoding.ASCII.GetBytes(e.Text)[0];
        var charData = TextAsCharList[_currentIndex];
            
        if (charData.Char == keyChar)
        {
            Console.WriteLine("Correct!");
            charData.Correct = true;
            _currentIndex++;
            return;
        }
        
        Console.WriteLine("Wrong! " + keyChar + " should be: " + charData.Char);
        charData.Wrong = true;
    }

}