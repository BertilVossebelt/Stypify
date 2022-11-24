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
        public NewGroupCodeCommand(Group newGroup, NavigationStore navigationStore,User user)
        {
            _group = newGroup;
            _user = user;
            _navigationStore = navigationStore;
        }

        public override void Execute(object? parameter)
        {
            _group.GroupCodeGeneratorMethod();
            System.Console.WriteLine(_group.GroupCode);
            
        }
    }

}

