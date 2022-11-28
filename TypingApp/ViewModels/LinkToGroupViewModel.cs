using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Stores;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Group = TypingApp.Models.Group;

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


        public LinkToGroupViewModel(NavigationStore navigationStore, User user, DatabaseConnection connection)
        {
            _groupCodeGroup = new Group(connection);

            BackButton = new BackCommand(navigationStore, user, connection);
            LinkToGroupSaveButton = new LinkToGroupSaveCommand(_groupCodeGroup, user, navigationStore, connection);
        }

    }
}
