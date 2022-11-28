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
    public ICommand AddToGroupButton { get; }

    public StudentDashboardViewModel(NavigationService exerciseNavigationService, NavigationService linkToGroupNavigationService)
    { 
        StartPracticeButton = new NavigateCommand(exerciseNavigationService);
        AddToGroupButton = new NavigateCommand(linkToGroupNavigationService);
    }
}