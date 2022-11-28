using System.Windows.Input;
using TypingApp.Models;
using TypingApp.Commands;
using TypingApp.Stores;
using System;
using TypingApp.Services;

namespace TypingApp.ViewModels;

public class StudentDashboardViewModel : ViewModelBase
{
    public ICommand StartPracticeButton { get; }
    
    public StudentDashboardViewModel(NavigationService exerciseNavigationService) 
    {
        StartPracticeButton = new NavigateCommand(exerciseNavigationService);
    }
}