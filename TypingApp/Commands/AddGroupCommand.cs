using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Commands
{
    internal class AddGroupCommand : CommandBase
    {
        private readonly User _user;
        private readonly NavigationStore _navigationStore;
        public AddGroupCommand(User user, NavigationStore navigationStore)
        {
            _user = user;
            _navigationStore = navigationStore;
        }

        public override void Execute(object? parameter)
        {
            Group newGroup = new Group();
            _navigationStore.CurrentViewModel = new AddGroupViewModel(newGroup ,_navigationStore, _user);
        }
    }

}

