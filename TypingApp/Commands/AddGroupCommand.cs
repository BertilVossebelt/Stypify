using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Commands
{
    internal class AddGroupCommand : CommandBase
    {
        private readonly User _user;
        private readonly NavigationStore _navigationStore;
        private readonly DatabaseConnection _connection;
        public AddGroupCommand(User user, NavigationStore navigationStore, DatabaseConnection connection)
        {
            _user = user;
            _navigationStore = navigationStore;
            _connection = connection;
        }

        public override void Execute(object? parameter)
        {
            Group newGroup = new Group(_connection);
            _navigationStore.CurrentViewModel = new AddGroupViewModel(newGroup ,_navigationStore, _user, _connection);
        }
    }

}

