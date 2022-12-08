using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Stores;
using Group = TypingApp.Models.Group;
using NavigationService = TypingApp.Services.NavigationService;

namespace TypingApp.ViewModels;

public class LinkToGroupViewModel : ViewModelBase
{
    private string _groupCode { get; set; }
    private string _groupName { get; set; }

    public ICommand LinkToGroupSaveButton { get; }
    public ICommand BackButton { get; }

    public string GroupName
    {
        get => _groupName;
        set
        {
            _groupName = value;
            OnPropertyChanged();
        }
    }
    
    public string GroupCode
    {
        get => _groupCode;
        set
        {
            _groupCode = value;
            OnPropertyChanged();
        }
    }


    public LinkToGroupViewModel(NavigationService studentDashboardNavigationService,
        NavigationService teacherDashboardNavigationService, UserStore userStore)
    {
        var teacher = new NavigateCommand(teacherDashboardNavigationService);
        var student = new NavigateCommand(studentDashboardNavigationService);
        BackButton = userStore.Teacher == null ? student : teacher;

        LinkToGroupSaveButton =
            new LinkToGroupSaveCommand(this, userStore, studentDashboardNavigationService);
    }
}