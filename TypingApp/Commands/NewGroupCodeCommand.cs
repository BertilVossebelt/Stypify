using System;
using System.Windows.Controls;
using System.Windows.Data;
using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;
using TypingApp.Views;

namespace TypingApp.Commands
{
    internal class NewGroupCodeCommand : CommandBase
    {
        private readonly User _user;
        private Group _group;
        private readonly DatabaseConnection _connection;
        private AddGroupViewModel _addGroupViewModel;
        public NewGroupCodeCommand(Group newGroup,User user, DatabaseConnection connection,AddGroupViewModel addGroupViewModel)
        {
            _group = newGroup;
            _user = user;
            _connection = connection;
            _addGroupViewModel = addGroupViewModel;
        }

        public override void Execute(object? parameter)
        {
            _group.GroupCodeGeneratorMethod();
            _addGroupViewModel.GroupCodeText = _group.GroupCode;
            Console.WriteLine(_group.GroupCode);
        }
    }

}

