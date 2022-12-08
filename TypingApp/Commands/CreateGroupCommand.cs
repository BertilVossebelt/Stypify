using System;
using System.Windows;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;

namespace TypingApp.Commands;

public class CreateGroupCommand : CommandBase
{
    private NavigationService _teacherDashboardNavigationService;
    private readonly UserStore _userStore;
    private readonly Group _group;

    public CreateGroupCommand(UserStore userStore, NavigationService teacherDashboardNavigationService,
        Group group)
    {
        _userStore = userStore;
        _teacherDashboardNavigationService = teacherDashboardNavigationService;
        _group = group;

    }

    public override void Execute(object? parameter)
    {
        if (_group.GroupName is "" or null)
        {
            MessageBox.Show("Je moet een naam invullen", "Geen naam", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var saveMessageBox = MessageBox.Show("Weet je zeker dat je deze groep wilt opslaan", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question);
        
        if (saveMessageBox != MessageBoxResult.Yes) return;
        if(_userStore.Teacher == null) return;

        //Save here to database
        new GroupProvider().Create(_userStore.Teacher.Id, _group.GroupName, _group.GroupCode);
        
        var navigateCommand = new NavigateCommand(_teacherDashboardNavigationService);
        navigateCommand.Execute(this);
    }
}