using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Commands
{
    internal class LinkToGroupCommand : CommandBase
    {
        private readonly User _user;
        private readonly NavigationStore _navigationStore;
        private readonly DatabaseConnection _connection;
        public LinkToGroupCommand(User user, NavigationStore navigationStore, DatabaseConnection connection)
        {
            _user = user;
            _navigationStore = navigationStore;
            _connection = connection;
        }

        public override void Execute(object? parameter)
        {
            _navigationStore.CurrentViewModel = new LinkToGroupViewModel(_navigationStore, _user, _connection);
        }
    }

}

