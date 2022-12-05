using TypingApp.Models;
using TypingApp.Services;

namespace TypingApp.Commands
{
    internal class AddGroupCommand : CommandBase
    {
        private readonly DatabaseConnection _connection;
        private NavigationService _addGroupNavigationService;

        public AddGroupCommand(DatabaseConnection connection, NavigationService addGroupNavigationService)
        {
            _connection = connection;
            _addGroupNavigationService = addGroupNavigationService;
        }

        public override void Execute(object? parameter)
        {
            var newGroup = new Group(_connection);
            newGroup.GroupCodeGeneratorMethod();

            var navigateCommand = new NavigateCommand(_addGroupNavigationService);
            navigateCommand.Execute(this);
        }
    }

}

