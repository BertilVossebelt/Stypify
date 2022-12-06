using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Services.Database;
using Group = TypingApp.Models.Group;
using NavigationService = TypingApp.Services.NavigationService;

namespace TypingApp.ViewModels
{
    public class LinkToGroupViewModel : ViewModelBase
    {
        public ICommand LinkToGroupSaveButton { get; }
        public ICommand BackButton { get; }
        private Group _groupCodeGroup { get; set; }
        
        private string _groupNameText { get; set; }
        public string GroupNameText
        {
            get => _groupNameText;
            set
            {
                _groupNameText = value;
                _groupCodeGroup.GroupCode = value;
                OnPropertyChanged();
            }
        }


        public LinkToGroupViewModel(NavigationService studentDashboardNavigationService, NavigationService teacherDashboardNavigationService, User user, DatabaseService connection)
        {
            _groupCodeGroup = new Group(connection);

            var teacher = new NavigateCommand(teacherDashboardNavigationService);
            var student = new NavigateCommand(studentDashboardNavigationService);
            BackButton = user.IsTeacher ? student : teacher;

            LinkToGroupSaveButton = new LinkToGroupSaveCommand(_groupCodeGroup, user, connection, studentDashboardNavigationService);
        }

    }
}
