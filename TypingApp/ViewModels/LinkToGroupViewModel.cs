using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Stores;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
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


        public LinkToGroupViewModel(NavigationService studentDashboardNavigationService, NavigationService teacherDashboardNavigationService, User user, DatabaseConnection connection)
        {
            _groupCodeGroup = new Group(connection);

            var teacher = new NavigateCommand(teacherDashboardNavigationService);
            var student = new NavigateCommand(studentDashboardNavigationService);
            BackButton = user.IsTeacher ? teacher : student;

            LinkToGroupSaveButton = new LinkToGroupSaveCommand(_groupCodeGroup, user, connection, studentDashboardNavigationService);
        }

    }
}
