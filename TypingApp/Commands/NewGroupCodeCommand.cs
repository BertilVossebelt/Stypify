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
        private readonly NavigationStore _navigationStore;
        private Group _group;
        private readonly DatabaseConnection _connection;
        private AddGroupViewModel _addGroupViewModel;
        public NewGroupCodeCommand(Group newGroup, NavigationStore navigationStore,User user, DatabaseConnection connection,AddGroupViewModel addGroupViewModel)
        {
            _group = newGroup;
            _user = user;
            _navigationStore = navigationStore;
            _connection = connection;
            _addGroupViewModel = addGroupViewModel;
        }

        public override void Execute(object? parameter)
        {
            _group.GroupCodeGeneratorMethod();
            _addGroupViewModel.GroupCodeText = _group.GroupCode;


            System.Console.WriteLine(_group.GroupCode);
            string QueryString3 = $"SELECT id,teacher_id,name,code FROM Groups WHERE teacher_id='{_user.Id}'";

            var reader = _connection.ExecuteSqlStatement(QueryString3);
            while (reader.Read())
            {
                Console.WriteLine($"{reader["id"]} {reader["teacher_id"]} {reader["name"]} {reader["code"]}");
            }
            reader.Close();
        }
    }

}

