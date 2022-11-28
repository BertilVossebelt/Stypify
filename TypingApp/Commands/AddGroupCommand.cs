using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Commands
{
    internal class AddGroupCommand : CommandBase
    {
        private readonly User _user;
        private readonly DatabaseConnection _connection;
        private NavigationService _addGroupNavigationService;

        public AddGroupCommand(User user, DatabaseConnection connection, NavigationService addGroupNavigationService)
        {
            _user = user;
            _connection = connection;
            _addGroupNavigationService = addGroupNavigationService;
        }

        public override void Execute(object? parameter)
        {
            Group newGroup = new Group(_connection);
            newGroup.GroupCodeGeneratorMethod();

            var navigateCommand = new NavigateCommand(_addGroupNavigationService);
            navigateCommand.Execute(this);
        }
    }

}

