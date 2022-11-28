using System;
using NavigationService = TypingApp.Services.NavigationService;

namespace TypingApp.Commands;

public class NavigateCommand : CommandBase
{
    private readonly Services.NavigationService _navigationService;
    public NavigateCommand(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public override void Execute(object? parameter)
    {
        _navigationService.Navigate();
    }
}